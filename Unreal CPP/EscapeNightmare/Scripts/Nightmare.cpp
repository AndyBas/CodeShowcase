// Fill out your copyright notice in the Description page of Project Settings.


#include "Nightmare.h"
#include "Components/StaticMeshComponent.h"
#include "SevenDFPSCharacter.h"

ANightmare::ANightmare()
{
	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	Mesh = GetStaticMeshComponent();

	SetMobility(EComponentMobility::Movable);

}

void ANightmare::BeginPlay()
{
	Super::BeginPlay();

	SetActorTickEnabled(true);

	Mesh->SetCollisionEnabled(ECollisionEnabled::QueryOnly);
	Mesh->SetCollisionResponseToAllChannels(ECollisionResponse::ECR_Overlap);
	Mesh->SetGenerateOverlapEvents(true);

	Mesh->OnComponentBeginOverlap.AddDynamic(this, &ANightmare::OnActorOverlapBegin);
	Mesh->OnComponentEndOverlap.AddDynamic(this, &ANightmare::OnActorOverlapEnd);
}

void ANightmare::Expand(float DeltaTime)
{
	float lDeltaExpanding = m_ExpandingSpeed * DeltaTime;
	Mesh->SetRelativeScale3D(Mesh->GetRelativeScale3D() + FVector(lDeltaExpanding, lDeltaExpanding, 0.f));
}

void ANightmare::Tick(float DeltaSeconds)
{
	Super::Tick(DeltaSeconds);
	Expand(DeltaSeconds);
}

#pragma region Collisions
void ANightmare::OnActorOverlapBegin(class UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	UE_LOG(LogTemp, Warning, TEXT("OverlapBegin !"));
	if (OtherActor)
	{
		ASevenDFPSCharacter* lChara = Cast<ASevenDFPSCharacter>(OtherActor);

		if (lChara)
			lChara->Caught(m_Damage);
	}
}

void ANightmare::OnActorOverlapEnd(class UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex)
{

	if (OtherActor)
	{
		ASevenDFPSCharacter* lChara = Cast<ASevenDFPSCharacter>(OtherActor);

		if (lChara)
			lChara->Fled();
	}
}

#pragma endregion Collisions

