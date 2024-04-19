#include "AbilitySystem/Abilities/LyraGameplayAbility_PhantomDash.h"
#include "GameFramework/Character.h"
#include "GameFramework/Controller.h"

ULyraGameplayAbility_PhantomDash::ULyraGameplayAbility_PhantomDash()
{
    DashForce = 600.f;
    InstancingPolicy = EGameplayAbilityInstancingPolicy::InstancedPerActor;
}

void ULyraGameplayAbility_PhantomDash::ActivateAbility(const FGameplayAbilitySpecHandle Handle, const FGameplayAbilityActorInfo* ActorInfo, const FGameplayAbilityActivationInfo ActivationInfo, const FGameplayEventData* TriggerEventData)
{
    // Ends the ability on the server to prevent desync and cheating
    if (HasAuthority(&ActivationInfo))
    {
        EndAbility(Handle, ActorInfo, ActivationInfo, true, true);
        if (GetWorld()->IsNetMode(NM_DedicatedServer))
        {
            return;
        }
    }
    else
    {
        return;
    }

    // On local client
    if (ACharacter* MyCharacter = Cast<ACharacter>(ActorInfo->AvatarActor.Get()))
    {
        // Applying a impulse to the character based on the player's movement input
        FVector InputVector = MyCharacter->GetLastMovementInputVector();
        if (!InputVector.IsNearlyZero())
        {
            FVector MovingInputDir = InputVector.GetSafeNormal();
            MyCharacter->LaunchCharacter(MovingInputDir, true, true);
        }
        else
        {
            // If movement is invalid, apply a forward impluse instead
            FVector DashDirection = MyCharacter->GetActorForwardVector() * DashForce;
            MyCharacter->LaunchCharacter(DashDirection, true, true);
        }
      
        CommitAbility(Handle, ActorInfo, ActivationInfo);
    }
    EndAbility(Handle, ActorInfo, ActivationInfo, true, false);
}