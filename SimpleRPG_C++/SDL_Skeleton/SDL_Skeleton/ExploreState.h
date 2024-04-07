#pragma once
#include "GameState.h"
#include "Map.h"

class ExploreState :
	public GameState
{
	b2World* m_pWorld;

	Map* m_pCurrentMap;
	CharacterBase* m_pPlayer;

public:
	ExploreState(SDL_Renderer* pRenderer, CharacterBase* player, b2World* pWorld);
	void Draw() override;
	~ExploreState();
	void SetMap(Map::EMapType maptype);
	void GetInput(const SDL_Event& event) override;
	bool Update(float deltaSeconds) override { return true; };
};
