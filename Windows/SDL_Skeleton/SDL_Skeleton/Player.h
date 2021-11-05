#pragma once
#include "CharacterBase.h"


class Player : public CharacterBase
{
public:
	Player(SDL_Renderer* pRenderer, ECharacterImageType type, b2World* pWorld);
	~Player();
	void Init() override;
	void Update(float deltaSeconds) override;

	void OnTriggerEnter(b2Fixture* pOther) override;
	void OnCollisionEnter(b2Fixture* pOther) override;
	void OnContactEnd(b2Fixture* pOther) override;
};

