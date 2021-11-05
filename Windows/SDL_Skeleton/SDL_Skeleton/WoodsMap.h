#pragma once
#include "Map.h"
class WoodsMap :
	public Map
{
	Tile* m_pGrassFloor;

public:
	WoodsMap(SDL_Renderer* pRenderer, b2World* pWorld);
	~WoodsMap();

	// Inherited via Map
	virtual void Draw(CharacterBase* m_pPlayer) override;
};

