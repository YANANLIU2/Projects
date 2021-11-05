#include "Map.h"
#include "Player.h"
#include "TiledObject.h"

Map::Map(SDL_Renderer* pRenderer, b2World* pWorld) :m_TreeTilesWH({8,10})
{
	m_pRenderer = pRenderer;
	m_pWorld = pWorld;
}

Map::~Map()
{
	Clean();
}

void Map::Clean()
{
	for (GameObject* i : m_pGameObjects)
	{
		delete i;
		i = nullptr;
	}

	m_pGameObjects.clear();

	for (GameObject* i : m_pCoverObjects)
	{
		delete i;
		i = nullptr;
	}

	m_pCoverObjects.clear();
}


