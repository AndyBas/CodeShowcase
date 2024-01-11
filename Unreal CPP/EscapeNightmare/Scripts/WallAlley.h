// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "WallAlley.generated.h"

UCLASS()
class SEVENDFPS_API AWallAlley : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AWallAlley();

	UFUNCTION()
		FORCEINLINE FVector GetWorldEndLoc() { return EndLoc; };

	UFUNCTION(BlueprintCallable, Category=Location)
	void SetEndLoc(FVector Loc);

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;


private:
	FVector EndLoc;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
