// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "SevenDFPSCharacter.generated.h"

class UInputComponent;
class USkeletalMeshComponent;
class USceneComponent;
class UCameraComponent;
class UAnimMontage;
class USoundBase;

// Declaration of the delegate that will be called when the Primary Action is triggered
// It is declared as dynamic so it can be accessed also in Blueprints
DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnUseItem);
DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnDied);

UCLASS(config=Game)
class ASevenDFPSCharacter : public ACharacter
{
	GENERATED_BODY()

	/** Pawn mesh: 1st person view (arms; seen only by self) */
	UPROPERTY(VisibleDefaultsOnly, Category=Mesh)
	USkeletalMeshComponent* Mesh1P;

	/** First person camera */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
	UCameraComponent* FirstPersonCameraComponent;

public:
	ASevenDFPSCharacter();

	/**
	 *	Function called every frame on this Actor. Override this function to implement custom logic to be executed every frame.
	 *	Note that Tick is disabled by default, and you will need to check PrimaryActorTick.bCanEverTick is set to true to enable it.
	 *
	 *	@param	DeltaSeconds	Game time elapsed during last frame modified by the time dilation
	 */
	virtual void Tick(float DeltaSeconds) override;

	/** Base turn rate, in deg/sec. Other scaling may affect final turn rate. */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category=Camera)
	float TurnRateGamepad;

	/** Delegate to whom anyone can subscribe to receive this event */
	UPROPERTY(BlueprintAssignable, Category = "Interaction")
	FOnUseItem OnUseItem;

	/** Delegate to whom anyone can subscribe to receive this event */
	UPROPERTY(BlueprintAssignable, Category = "Life")
	FOnUseItem OnDied;

	UFUNCTION(BlueprintCallable, Category= "Nightmare")
	void Caught(float Damage);

	UFUNCTION(BlueprintCallable, Category= "Nightmare")
	void Fled();

	UFUNCTION()
	void StopBehaving();

	UFUNCTION(BlueprintPure, Category="Life")
	FORCEINLINE float GetLifeRatio() { return m_CurLife / m_MaxLife; };

	UFUNCTION(BlueprintPure, Category="Stamina")
	FORCEINLINE float GetStaminaRatio() { return m_CurStamina / m_MaxStamina; };

protected:
	
	virtual void BeginPlay();

	/** Handles moving forward/backward */
	void MoveForward(float Val);

	/** Handles strafing movement, left and right */
	void MoveRight(float Val);

	/**
	 * Called via input to turn at a given rate.
	 * @param Rate	This is a normalized rate, i.e. 1.0 means 100% of desired turn rate
	 */
	void TurnAtRate(float Rate);

	/**
	 * Called via input to turn look up/down at a given rate.
	 * @param Rate	This is a normalized rate, i.e. 1.0 means 100% of desired turn rate
	 */
	void LookUpAtRate(float Rate);


	UFUNCTION(BlueprintCallable, Category = Character)
	void Sprint();

	UFUNCTION(BlueprintCallable, Category = Character)
	void StopSprinting();

	UPROPERTY(VisibleAnywhere, Category = Character)
	bool bIsSprinting;

	struct TouchData
	{
		TouchData() { bIsPressed = false;Location=FVector::ZeroVector;}
		bool bIsPressed;
		ETouchIndex::Type FingerIndex;
		FVector Location;
		bool bMoved;
	};
	void BeginTouch(const ETouchIndex::Type FingerIndex, const FVector Location);
	void EndTouch(const ETouchIndex::Type FingerIndex, const FVector Location);
	void TouchUpdate(const ETouchIndex::Type FingerIndex, const FVector Location);
	TouchData	TouchItem;

	// APawn interface
	virtual void SetupPlayerInputComponent(UInputComponent* InputComponent) override;
	//virtual void DestroyPlayerInputComponent() override;
	// End of APawn interface

	UFUNCTION()
	void OnPrimaryAction();

	UFUNCTION(BlueprintNativeEvent)
	void TraceForward();
	void TraceForward_Implementation();

	/* 
	 * Configures input for touchscreen devices if there is a valid touch interface for doing so 
	 *
	 * @param	InputComponent	The input component pointer to bind controls to
	 * @returns true if touch controls were enabled.
	 */
	bool EnableTouchscreenMovement(UInputComponent* InputComponent);

	UFUNCTION(BlueprintCallable, Category= "Damage")
	void InflictDamage(float DamageTaken);


	UFUNCTION(BlueprintCallable, Category= "Life")
	void RestoreLife(float LifeToGive);

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Interact")
	float TraceDistance;

private:
	/*
	* ==============================================
	*					FUNCTIONS
	* ==============================================
	*/
	UFUNCTION(BlueprintCallable, Category=Stamina)
	void DecreaseStamina(float StaminaLooseVal);

	UFUNCTION(BlueprintCallable, Category=Stamina)
	void RegainStamina(float StaminaRecoveryVal);

	/*
	* ==============================================
	*					FIELDS
	* ==============================================
	*/

	UPROPERTY()
	bool bDied = false;

	UPROPERTY()
	bool bCaughtByNightmare = false;

	// LIFE
	UPROPERTY(VisibleAnywhere, Category= "Life")
	float m_CurLife;

	UPROPERTY(EditAnywhere, Category= "Life")
	float m_MaxLife = 100.f;

	UPROPERTY(EditAnywhere, Category= "Life")
	float m_LifeRecoverySpeed = 5.f;

	float m_DamageFromNightmare = 0.f;

	// STAMINA
	UPROPERTY(VisibleAnywhere, Category= "Stamina")
	float m_CurStamina;

	UPROPERTY(EditAnywhere, Category= "Stamina")
	float m_MaxStamina = 100.f;

	UPROPERTY(EditAnywhere, Category= "Stamina")
	float m_StaminaRecoverySpeed = 25.f;

	UPROPERTY(EditAnywhere, Category= "Stamina")
	float m_StaminaLooseSpeed = 35.f;

	UPROPERTY(EditAnywhere, Category= "Character")
	float m_WalkSpeed = 1000.f;

	UPROPERTY(EditAnywhere, Category= "Character")
	float m_SprintSpeed = 2000.f;

	UPROPERTY(EditAnywhere, Category= "Behavior")
	bool bGameRunning;

	UPROPERTY(EditAnywhere, Category= "Debug")
	bool bDebugMode;

public:
	/** Returns Mesh1P subobject **/
	USkeletalMeshComponent* GetMesh1P() const { return Mesh1P; }
	/** Returns FirstPersonCameraComponent subobject **/
	UCameraComponent* GetFirstPersonCameraComponent() const { return FirstPersonCameraComponent; }

};

