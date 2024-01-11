// Copyright Epic Games, Inc. All Rights Reserved.

#include "SevenDFPSCharacter.h"
#include "SevenDFPSProjectile.h"
#include "Animation/AnimInstance.h"
#include "Camera/CameraComponent.h"
#include "Components/CapsuleComponent.h"
#include "Components/InputComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "GameFramework/InputSettings.h"
#include "Door.h"


//////////////////////////////////////////////////////////////////////////
// ASevenDFPSCharacter

ASevenDFPSCharacter::ASevenDFPSCharacter() 
{
	// Set size for collision capsule
	GetCapsuleComponent()->InitCapsuleSize(55.f, 96.0f);

	// set our turn rates for input
	TurnRateGamepad = 45.f;

	// Create a CameraComponent	
	FirstPersonCameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("FirstPersonCamera"));
	FirstPersonCameraComponent->SetupAttachment(GetCapsuleComponent());
	FirstPersonCameraComponent->SetRelativeLocation(FVector(-39.56f, 1.75f, 64.f)); // Position the camera
	FirstPersonCameraComponent->bUsePawnControlRotation = true;

	// Create a mesh component that will be used when being viewed from a '1st person' view (when controlling this pawn)
	Mesh1P = CreateDefaultSubobject<USkeletalMeshComponent>(TEXT("CharacterMesh1P"));
	Mesh1P->SetOnlyOwnerSee(true);
	Mesh1P->SetupAttachment(FirstPersonCameraComponent);
	Mesh1P->bCastDynamicShadow = false;
	Mesh1P->CastShadow = false;
	Mesh1P->SetRelativeRotation(FRotator(1.9f, -19.19f, 5.2f));
	Mesh1P->SetRelativeLocation(FVector(-0.5f, -4.4f, -155.7f));

	TraceDistance = 200.f;
	bGameRunning = true;
}

void ASevenDFPSCharacter::BeginPlay()
{
	// Call the base class  
	Super::BeginPlay();

	m_CurLife = m_MaxLife;
	m_CurStamina = m_MaxStamina;
}

void ASevenDFPSCharacter::Tick(float DeltaSeconds)
{
	Super::Tick(DeltaSeconds);

	if (!bGameRunning) return;

	if (bCaughtByNightmare)
		InflictDamage(DeltaSeconds * m_DamageFromNightmare);
	else RestoreLife(DeltaSeconds * m_LifeRecoverySpeed);


	// Sprinting handling
	if (bIsSprinting)
		DecreaseStamina(DeltaSeconds * m_StaminaLooseSpeed);
	else if (m_CurStamina < m_MaxStamina)
		RegainStamina(DeltaSeconds * m_StaminaRecoverySpeed);
}


//////////////////////////////////////////////////////////////////////////// Input

void ASevenDFPSCharacter::SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent)
{
	// Set up gameplay key bindings
	check(PlayerInputComponent);

	// Bind jump events
	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &ACharacter::Jump);
	PlayerInputComponent->BindAction("Jump", IE_Released, this, &ACharacter::StopJumping);

	// Bind run events
	PlayerInputComponent->BindAction("Sprint", IE_Pressed, this, &ASevenDFPSCharacter::Sprint);
	PlayerInputComponent->BindAction("Sprint", IE_Released, this, &ASevenDFPSCharacter::StopSprinting);

	// Bind fire event
	PlayerInputComponent->BindAction("PrimaryAction", IE_Pressed, this, &ASevenDFPSCharacter::OnPrimaryAction);

	// Enable touchscreen input
	EnableTouchscreenMovement(PlayerInputComponent);

	// Bind movement events
	PlayerInputComponent->BindAxis("Move Forward / Backward", this, &ASevenDFPSCharacter::MoveForward);
	PlayerInputComponent->BindAxis("Move Right / Left", this, &ASevenDFPSCharacter::MoveRight);

	// We have 2 versions of the rotation bindings to handle different kinds of devices differently
	// "Mouse" versions handle devices that provide an absolute delta, such as a mouse.
	// "Gamepad" versions are for devices that we choose to treat as a rate of change, such as an analog joystick
	PlayerInputComponent->BindAxis("Turn Right / Left Mouse", this, &APawn::AddControllerYawInput);
	PlayerInputComponent->BindAxis("Look Up / Down Mouse", this, &APawn::AddControllerPitchInput);
	PlayerInputComponent->BindAxis("Turn Right / Left Gamepad", this, &ASevenDFPSCharacter::TurnAtRate);
	PlayerInputComponent->BindAxis("Look Up / Down Gamepad", this, &ASevenDFPSCharacter::LookUpAtRate);
}

void ASevenDFPSCharacter::OnPrimaryAction()
{
	TraceForward();
}

void ASevenDFPSCharacter::TraceForward_Implementation()
{
	FVector lLoc;
	FRotator lRot;
	FHitResult lHit;

	GetController()->GetPlayerViewPoint(lLoc, lRot);

	FVector lStart = lLoc;
	FVector lEnd = lStart + (lRot.Vector() * TraceDistance);

	FCollisionQueryParams lTraceParams;
	bool bHit = GetWorld()->LineTraceSingleByChannel(lHit, lStart, lEnd, ECC_Visibility, lTraceParams);

	if(bDebugMode)
		DrawDebugLine(GetWorld(), lStart, lEnd, FColor::Orange, false, 2.f);

	if (bHit)
	{
		AActor* lHitActor = lHit.GetActor();

		if (lHitActor)
		{
			ADoor* lDoor = Cast<ADoor>(lHitActor);

			if (lDoor)
				lDoor->TryOpening();
		}

		if(bDebugMode)
			DrawDebugBox(GetWorld(), lHit.ImpactPoint, FVector(5, 5, 5), FColor::Emerald, false, 2.f);
	}
}


void ASevenDFPSCharacter::BeginTouch(const ETouchIndex::Type FingerIndex, const FVector Location)
{
	if (TouchItem.bIsPressed == true)
	{
		return;
	}
	if ((FingerIndex == TouchItem.FingerIndex) && (TouchItem.bMoved == false))
	{
		//OnPrimaryAction();
	}
	TouchItem.bIsPressed = true;
	TouchItem.FingerIndex = FingerIndex;
	TouchItem.Location = Location;
	TouchItem.bMoved = false;
}

void ASevenDFPSCharacter::EndTouch(const ETouchIndex::Type FingerIndex, const FVector Location)
{
	if (TouchItem.bIsPressed == false)
	{
		return;
	}
	TouchItem.bIsPressed = false;
}

void ASevenDFPSCharacter::Caught(float Damage)
{
	bCaughtByNightmare = true;
	m_DamageFromNightmare = Damage;
}

void ASevenDFPSCharacter::Fled()
{
	bCaughtByNightmare = false;
	m_DamageFromNightmare = 0.f;
}

void ASevenDFPSCharacter::MoveForward(float Value)
{
	if (Value != 0.0f)
	{
		// add movement in that direction
		AddMovementInput(GetActorForwardVector(), Value);
	}
}

void ASevenDFPSCharacter::MoveRight(float Value)
{
	if (Value != 0.0f)
	{
		// add movement in that direction
		AddMovementInput(GetActorRightVector(), Value);
	}
}

void ASevenDFPSCharacter::TurnAtRate(float Rate)
{
	// calculate delta for this frame from the rate information
	AddControllerYawInput(Rate * TurnRateGamepad * GetWorld()->GetDeltaSeconds());
}

void ASevenDFPSCharacter::LookUpAtRate(float Rate)
{
	// calculate delta for this frame from the rate information
	AddControllerPitchInput(Rate * TurnRateGamepad * GetWorld()->GetDeltaSeconds());
}

void ASevenDFPSCharacter::Sprint()
{
	bIsSprinting = true;
	GetCharacterMovement()->MaxWalkSpeed = m_SprintSpeed;
}

void ASevenDFPSCharacter::StopSprinting()
{
	bIsSprinting = false;
	GetCharacterMovement()->MaxWalkSpeed = m_WalkSpeed;
}

bool ASevenDFPSCharacter::EnableTouchscreenMovement(class UInputComponent* PlayerInputComponent)
{
	if (FPlatformMisc::SupportsTouchInput() || GetDefault<UInputSettings>()->bUseMouseForTouch)
	{
		PlayerInputComponent->BindTouch(EInputEvent::IE_Pressed, this, &ASevenDFPSCharacter::BeginTouch);
		PlayerInputComponent->BindTouch(EInputEvent::IE_Released, this, &ASevenDFPSCharacter::EndTouch);

		return true;
	}
	
	return false;
}

void ASevenDFPSCharacter::InflictDamage(float DamageTaken)
{
	if (m_CurLife > 0.f)
	{
		m_CurLife -= DamageTaken;

		if (m_CurLife <= 0.f)
			OnDied.Broadcast();
	}
}

void ASevenDFPSCharacter::RestoreLife(float LifeToGive)
{
	if (m_CurLife < m_MaxLife)
	{
		m_CurLife += LifeToGive;

		if (m_CurLife > m_MaxLife)
			m_CurLife = m_MaxLife;
	}
}

void ASevenDFPSCharacter::DecreaseStamina(float StaminaLooseVal)
{
	if (m_CurStamina > 0.f)
	{
		m_CurStamina -= StaminaLooseVal;

		if (m_CurStamina <= 0.f)
		{
			m_CurStamina = 0.f;
			StopSprinting();
		}
	}
}

void ASevenDFPSCharacter::RegainStamina(float StaminaRecoveryVal)
{
	if (m_CurStamina < m_MaxStamina)
	{
		m_CurStamina += StaminaRecoveryVal;

		if (m_CurStamina >= m_MaxStamina)
			m_CurStamina = m_MaxStamina;
	}
}

void ASevenDFPSCharacter::StopBehaving()
{
	bGameRunning = false;
	AController* lController = GetController();

	if (lController)
	{
		APlayerController* lPlayerC = Cast<APlayerController>(lController);
		if (lPlayerC)
		{
			lPlayerC->bShowMouseCursor = true;
			lPlayerC->bEnableClickEvents = true;
			lPlayerC->bEnableMouseOverEvents = true;
		}

	}

	DestroyPlayerInputComponent();
}
