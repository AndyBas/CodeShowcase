// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/StaticMeshActor.h"
#include "Door.generated.h"

/**
 * 
 */
UCLASS()
class SEVENDFPS_API ADoor : public AStaticMeshActor
{
	GENERATED_BODY()
	
public:
	UFUNCTION(BlueprintCallable, Category = "Opening")
	void TryOpening();

	UFUNCTION(BlueprintNativeEvent, Category= "Opening")
	void Open();
	void Open_Implementation();

protected:
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category= "Opening")
	bool bCanBeOpen;
};
