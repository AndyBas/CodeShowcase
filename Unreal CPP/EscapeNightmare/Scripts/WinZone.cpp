// Fill out your copyright notice in the Description page of Project Settings.


#include "WinZone.h"
#include "SevenDFPSCharacter.h"

AWinZone::AWinZone()
{

	Mesh = GetStaticMeshComponent();

	SetMobility(EComponentMobility::Movable);
}

AWinZone::~AWinZone()
{
}

void AWinZone::BeginPlay()
{

	Mesh->SetCollisionEnabled(ECollisionEnabled::QueryOnly);
	Mesh->SetCollisionResponseToAllChannels(ECollisionResponse::ECR_Overlap);
	Mesh->SetGenerateOverlapEvents(true);

	Mesh->OnComponentBeginOverlap.AddDynamic(this, &AWinZone::OnActorOverlapBegin);
}

void AWinZone::OnActorOverlapBegin(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	if (OtherActor)
	{
		ASevenDFPSCharacter* lChara = Cast<ASevenDFPSCharacter>(OtherActor);
		if (lChara)
		{
			OnCollided.Broadcast();
			Mesh->OnComponentBeginOverlap.RemoveDynamic(this, &AWinZone::OnActorOverlapBegin);
		}
	}

}
