// Fill out your copyright notice in the Description page of Project Settings.


#include "WallAlley.h"

// Sets default values
AWallAlley::AWallAlley()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

void AWallAlley::SetEndLoc(FVector Loc)
{
	EndLoc = Loc;
}

// Called when the game starts or when spawned
void AWallAlley::BeginPlay()
{
	Super::BeginPlay();
	

}

// Called every frame
void AWallAlley::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

