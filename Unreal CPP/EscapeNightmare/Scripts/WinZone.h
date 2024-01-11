// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/StaticMeshActor.h"
#include "WinZone.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnCollided);
/**
 * 
 */
UCLASS()
class SEVENDFPS_API AWinZone : public AStaticMeshActor
{
	GENERATED_BODY()
	
public:

	/** Delegate to whom anyone can subscribe to receive this event */
	UPROPERTY(BlueprintAssignable, Category = "WinEvent")
	FOnCollided OnCollided;

	AWinZone();
	~AWinZone();
protected:
	virtual void BeginPlay() override;

	UFUNCTION()
	void OnActorOverlapBegin(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	class UStaticMeshComponent* Mesh;
};
