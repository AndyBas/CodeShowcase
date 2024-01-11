// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/StaticMeshActor.h"
#include "Nightmare.generated.h"

/**
 * 
 */
UCLASS()
class SEVENDFPS_API ANightmare : public AStaticMeshActor
{
	GENERATED_BODY()

public:
	ANightmare();


	/**
	 *	Function called every frame on this Actor. Override this function to implement custom logic to be executed every frame.
	 *	Note that Tick is disabled by default, and you will need to check PrimaryActorTick.bCanEverTick is set to true to enable it.
	 *
	 *	@param	DeltaSeconds	Game time elapsed during last frame modified by the time dilation
	 */
	virtual void Tick(float DeltaSeconds) override;

	UFUNCTION()
	void OnActorOverlapBegin(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	UFUNCTION()
	void OnActorOverlapEnd(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);

	UFUNCTION()
	FORCEINLINE void SetShouldExpand(bool ShouldExpand) { bShouldExpand = ShouldExpand; };

protected:
	virtual void BeginPlay() override;

	UFUNCTION(BlueprintCallable, Category= "Expansion")
	void Expand(float DeltaTime);

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
	class UStaticMeshComponent* Mesh;

	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category="Expansion")
	float m_ExpandingSpeed = 20.f;

	UPROPERTY(VisibleAnywhere, Category= Expansion)
	bool bShouldExpand = false;

	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category="Damage")
	float m_Damage = 5.f;
};
