// Copyright Epic Games, Inc. All Rights Reserved.

#include "SevenDFPSGameMode.h"
#include "UObject/ConstructorHelpers.h"
#include "Kismet/GameplayStatics.h"
#include "Kismet/KismetSystemLibrary.h"
#include "Kismet/GameplayStatics.h"
#include "HUDSDFPS.h"
#include "WinZone.h"
#include "Nightmare.h"

ASevenDFPSGameMode::ASevenDFPSGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}

ASevenDFPSGameMode::~ASevenDFPSGameMode()
{
	//m_Chara->OnDied.RemoveDynamic(this, &ASevenDFPSGameMode::PlayerDied);
	//AWinZone::OnCollided.RemoveDynamic(this, ASevenDFPSGameMode::OnGameWin);
}

void ASevenDFPSGameMode::BeginPlay()
{
	Super::BeginPlay();

	// Access the wrld to get to the players
	UWorld* lWorld = GetWorld();
	check(lWorld);

	AActor* lCharaActor =  UGameplayStatics::GetActorOfClass(lWorld, ASevenDFPSCharacter::StaticClass());
	check(lCharaActor);

	m_Chara = Cast<ASevenDFPSCharacter>(lCharaActor);

	m_Chara->OnDied.AddDynamic(this, &ASevenDFPSGameMode::PlayerDied);

	AActor* lWinZone = UGameplayStatics::GetActorOfClass(lWorld, AWinZone::StaticClass());
	check(lWinZone);

	m_WinZone = Cast<AWinZone>(lWinZone);
	m_WinZone->OnCollided.AddDynamic(this, &ASevenDFPSGameMode::OnGameWin);

	AActor* lNightmare = UGameplayStatics::GetActorOfClass(lWorld, ANightmare::StaticClass());
	check(lNightmare);

	m_Nightmare = Cast<ANightmare>(lNightmare);

	m_HUD = Cast<AHUDSDFPS>(lWorld->GetFirstPlayerController()->GetHUD());
	check(m_HUD);
}

void ASevenDFPSGameMode::PlayerDied()
{
	m_Chara->OnDied.RemoveDynamic(this, &ASevenDFPSGameMode::PlayerDied);

	StopGame();
	m_HUD->DisplayGameOverScreen();
	UE_LOG(LogTemp, Warning, TEXT("DIED"));

	m_HUD->OnGameOverScreenChange.AddDynamic(this, &ASevenDFPSGameMode::Retry);
}

void ASevenDFPSGameMode::OnGameWin()
{
	m_WinZone->OnCollided.RemoveDynamic(this, &ASevenDFPSGameMode::OnGameWin);

	StopGame();
	m_HUD->DisplayWinScreen();

	m_HUD->OnWinScreenChange.AddDynamic(this, &ASevenDFPSGameMode::CloseGameWin);
}

void ASevenDFPSGameMode::StopGame()
{
	m_Nightmare->SetShouldExpand(false);
	m_Chara->StopBehaving();
}

void ASevenDFPSGameMode::CloseGame()
{
	const UWorld* lWorld = GetWorld();
	check(lWorld);

	UKismetSystemLibrary::QuitGame(lWorld, m_Chara->GetLocalViewingPlayerController(), EQuitPreference::Quit, true);
}

void ASevenDFPSGameMode::Retry()
{
	m_HUD->OnGameOverScreenChange.RemoveDynamic(this, &ASevenDFPSGameMode::Retry);

	const UWorld* lWorld = GetWorld();
	check(lWorld);

	UGameplayStatics::OpenLevel(lWorld, TEXT("L_Game"));
}

void ASevenDFPSGameMode::CloseGameWin()
{
	m_HUD->OnWinScreenChange.RemoveDynamic(this, &ASevenDFPSGameMode::CloseGameWin);

	UE_LOG(LogTemp, Warning, TEXT("please close !!"));
	CloseGame();
}
