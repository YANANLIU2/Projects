// The class is for providing functionality of a gameplay ability: PhanthomDash
// After pressed the input key, the player will dash to the moving direction or forward if the player is not moving atm.

#pragma once

#include "CoreMinimal.h"
#include "AbilitySystem/Abilities/LyraGameplayAbility.h"
#include "LyraGameplayAbility_PhantomDash.generated.h"

/**
 * 
 */
UCLASS()
class LYRAGAME_API ULyraGameplayAbility_PhantomDash : public ULyraGameplayAbility
{
	GENERATED_BODY()
public:
    ULyraGameplayAbility_PhantomDash();

    UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Dash")
        float DashForce;

protected:
    virtual void ActivateAbility(const FGameplayAbilitySpecHandle Handle, const FGameplayAbilityActorInfo* ActorInfo, const FGameplayAbilityActivationInfo ActivationInfo, const FGameplayEventData* TriggerEventData) override;
};