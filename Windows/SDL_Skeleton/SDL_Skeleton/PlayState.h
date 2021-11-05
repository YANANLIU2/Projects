#pragma once
#include "GameState.h"
#include "Tile.h"
#include "CollisionHandler.h"
#include "DebugDraw.h"

class b2World;
class CharacterBase;
class GameStateMachine;
class PlayState : public GameState
{
	CharacterBase* m_pPlayer;
	GameStateMachine* m_IngameStateMachine;
	GameState* m_explorState;

	CollisionHandler m_TheCollisionHandler;
	b2World* m_pWorld;
	DebugDraw m_pDebugDraw;

public:
	PlayState(SDL_Renderer* pRenderer);
	~PlayState();
	void Draw() override;
	bool Update(float deltaSeconds) override;
	void GetInput(const SDL_Event& event) override;
};

