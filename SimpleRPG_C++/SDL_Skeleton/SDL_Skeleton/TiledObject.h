#pragma once
#include "GameObject.h"
#include "SDL_render.h"

class Tile;
class TiledObject : public GameObject
{

	Tile** m_pPTiles;
	intpair m_SourceColRow;
	SDL_Renderer * m_pRenderer;

public:
	TiledObject(SDL_Renderer* pRenderer, ETileType tiletype, intpair widthHeight, intpair leftCorner, intpair SourceColRow, b2World* pWorld);
	~TiledObject();

	void DrawTiledMap();
	void InitATiledMap();
	void DeleteTiledMap();

	void OnTriggerEnter(b2Fixture* pOther) override;
	void OnContactEnd(b2Fixture* pOther) override;
	void Draw() override;
};

