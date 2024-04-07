#pragma once
#include "Tile.h"
#include <unordered_map>
#include "GameObject.h"
class CharacterBase;

class Map
{
public:
	enum EMapType
	{
		kWoods
	};

protected:
	const intpair m_FloorTilesWH = { WINDOW_WIDTH / k_tilesize ,WINDOW_HEIGHT / k_tilesize };
	const intpair m_TreeTilesWH;

	SDL_Renderer* m_pRenderer;

	CharacterBase* m_pPlayer;

	b2World* m_pWorld;
	std::vector<GameObject*> m_pGameObjects;
	std::vector<GameObject*> m_pCoverObjects;


public:
	Map(SDL_Renderer* pRenderer, b2World* pWorld);
	~Map();
	virtual void Draw(CharacterBase* m_pPlayer) {};
	void Clean();
};

