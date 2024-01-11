// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "HUDSDFPS.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnChangeScreen);

/**
 * 
 */
UCLASS()
class SEVENDFPS_API AHUDSDFPS : public AHUD
{
	GENERATED_BODY()

public:
	UPROPERTY(BlueprintAssignable, BlueprintCallable, Category= Screen)
	FOnChangeScreen OnWinScreenChange;

	UPROPERTY(BlueprintAssignable, BlueprintCallable,  Category= Screen)
	FOnChangeScreen OnGameOverScreenChange;
	
	UFUNCTION(BlueprintNativeEvent)
	void DisplayWinScreen();
	void DisplayWinScreen_Implementation();
	
	UFUNCTION(BlueprintNativeEvent)
	void DisplayGameOverScreen();
	void DisplayGameOverScreen_Implementation();
	
	UFUNCTION(BlueprintNativeEvent)
	void HideWinScreen();
	void HideWinScreen_Implementation();
	
	UFUNCTION(BlueprintNativeEvent)
	void HideGameOverScreen();
	void HideGameOverScreen_Implementation();
};
