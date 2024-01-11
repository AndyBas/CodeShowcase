// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "SevenDFPSCharacter.h"
#include "SevenDFPSGameMode.generated.h"

UCLASS(minimalapi)
class ASevenDFPSGameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	ASevenDFPSGameMode();
	~ASevenDFPSGameMode();

	virtual void BeginPlay() override;

	UFUNCTION(BlueprintPure, Category= PlayerLife)
	FORCEINLINE float GetPlayerLifeRatio() { return m_Chara->GetLifeRatio(); };

	UFUNCTION(BlueprintPure, Category= PlayerLife)
	FORCEINLINE float GetPlayerStaminaRatio() { return m_Chara->GetStaminaRatio(); };

private:

	UFUNCTION()
	void PlayerDied();

	UFUNCTION()
	void OnGameWin();

	UFUNCTION()
	void StopGame();

	UFUNCTION()
	void CloseGameWin();

	UFUNCTION()
	void Retry();

	UFUNCTION()
	void CloseGame();

	UPROPERTY()
	class ASevenDFPSCharacter* m_Chara;

	UPROPERTY()
	class AWinZone* m_WinZone;

	UPROPERTY()
	class ANightmare* m_Nightmare;

	UPROPERTY()
	class AHUDSDFPS* m_HUD;
	
	UPROPERTY(EditAnywhere, Category=Alleys)
	UClass* m_AlleySimpleClass;
	
	UPROPERTY(EditAnywhere, Category=Alleys)
	UClass* m_AlleyBranchClass;
	
	UPROPERTY(EditAnywhere, Category=Alleys)
	UClass* m_AlleyDoorClass;
	
	UPROPERTY(EditAnywhere, Category=Alleys)
	UClass* m_AlleyFakeDoorClass;
};



