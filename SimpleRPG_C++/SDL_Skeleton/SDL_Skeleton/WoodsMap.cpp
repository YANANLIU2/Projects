#include "WoodsMap.h"
#include "CharacterBase.h"
#include "TiledObject.h"

WoodsMap::WoodsMap(SDL_Renderer* pRenderer, b2World* pWorld):Map(pRenderer, pWorld)
{
	//Init tiles

	//floor 
	m_pGrassFloor = new Tile(m_pRenderer, ETileType::kFloor, Tile::m_pWoodSetTexture, { 0, 0, k_tilesize,k_tilesize }, { 0, 0, k_tilesize,k_tilesize });

	//trees
	m_pGameObjects.push_back(new TiledObject(m_pRenderer, ETileType::kTree, { 8,10 }, { 500,200 }, { 0,15 }, m_pWorld));
	m_pGameObjects.push_back(new GameObject(EGameObjectType::kBlock, { 2,3 }, { 500 + 3 * 32,200 + 6 * 32 }, m_pWorld, ETileType::kDefault));

	//Decorates

		//flower
	m_pGameObjects.push_back(new TiledObject(m_pRenderer, ETileType::kYellowFlower, { 1,1 }, { 800,100}, { 7,0 }, m_pWorld));

	
		//mushroom
	m_pGameObjects.push_back(new TiledObject(m_pRenderer, ETileType::kTree, { 1,1 }, { 999,500 }, { 4,2 }, m_pWorld));
}


WoodsMap::~WoodsMap()
{
	delete m_pGrassFloor;
	m_pGrassFloor = nullptr;
	Clean();
}

void WoodsMap::Draw(CharacterBase* m_pPlayer)
{
	//1st: floor
	for (int y = 0; y < m_FloorTilesWH.y; ++y)
	{
		for (int x = 0; x < m_FloorTilesWH.x; ++x)
		{
			m_pGrassFloor->SetDesRect({ x*k_tilesize, y*k_tilesize,k_tilesize,k_tilesize });
			m_pGrassFloor->Draw();
		}
	}

	for (auto i : m_pGameObjects)
	{
		if (i->GetHide() == false)
			i->Draw();
		else
			m_pCoverObjects.push_back(i);
	}

	m_pPlayer->Draw();

	for (auto i : m_pCoverObjects)
	{
		if (i->GetHide() == true)
			i->Draw();
	}

	m_pCoverObjects.clear();
}


