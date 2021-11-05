#include "TiledObject.h"
#include "Tile.h"
#include "CharacterBase.h"

TiledObject::TiledObject(SDL_Renderer* pRenderer, ETileType tiletype, intpair widthHeight, intpair leftCorner, intpair SourceColRow, b2World* pWorld)
	: GameObject(EGameObjectType::kTile, widthHeight, leftCorner, pWorld, tiletype)
{
	m_pRenderer = pRenderer;
	m_SourceColRow = SourceColRow;
	
	this->InitATiledMap();

	//set collision body
	this->SetCollisionBody();
}

TiledObject::~TiledObject()
{
	this->DeleteTiledMap();
	m_pBody->DestroyFixture(m_pBody->GetFixtureList());
}

void TiledObject::DrawTiledMap()
{
	for (int y = 0; y < m_WidthHeight.y; ++y)
	{
		for (int x = 0; x < m_WidthHeight.x; ++x)
		{
			m_pPTiles[y*m_WidthHeight.x + x]->Draw();
		}
	}
	

}

//Init any tiled map for the client. use the image stored in tiled (tiletype). 
//leftcorner is the position of the map. 
//width and height is the columns and rows of the tiled map. 
//sourceColROw is the wanted columns and wanted rows of the original image 
//repeat == true: repeat the whole pattern 
//repeat == false: the whole pattern once. 
void TiledObject::InitATiledMap()
{
	m_pPTiles = new Tile*[m_WidthHeight.x*m_WidthHeight.y];

	int currentCol = 0;
	int currentRow = 0;

	for (int j = 0; j < m_WidthHeight.y; ++j)
	{
		for (int i = 0; i < m_WidthHeight.x; ++i)
		{
			int index = j * m_WidthHeight.x + i;
			m_pPTiles[index] = new Tile(m_pRenderer, m_Tiletype, Tile::GetTexture(m_Tiletype,false), { (i + m_SourceColRow.x)*k_tilesize, (j + m_SourceColRow.y)*k_tilesize,k_tilesize,k_tilesize }, { i*k_tilesize + m_LeftCorner.x,j*k_tilesize + m_LeftCorner.y,k_tilesize,k_tilesize });
		}
	}
}

void TiledObject::DeleteTiledMap()
{
	for (int j = 0; j < m_WidthHeight.y; ++j)
	{
		for (int i = 0; i < m_WidthHeight.x; ++i)
		{
			int index = j*m_WidthHeight.x + i;
			delete m_pPTiles[index];
			m_pPTiles[index] = nullptr;
		}
	}
	delete[] m_pPTiles;
	m_pPTiles = nullptr;
}

void TiledObject::OnTriggerEnter(b2Fixture * pOther)
{
	// if player enter and .y < midpoint of obj. hide.

	if (CharacterBase* player = static_cast<CharacterBase*>(pOther->GetBody()->GetUserData()))
	{
		if (float(player->GetPosition().y + player->GetSize().y / 2.f) <= (m_LeftCorner.y + m_WidthHeight.y *k_tilesize / 2.f))
		{
			m_hideCharacter = true;

			for (int y = 0; y < m_WidthHeight.y; ++y)
			{
				for (int x = 0; x < m_WidthHeight.x; ++x)
				{
					if (m_hideCharacter)
					{
						m_pPTiles[y*m_WidthHeight.x + x]->SetTextures(Tile::GetTexture(m_Tiletype, true));
					}
				}
			}
		}
	}
}

void TiledObject::OnContactEnd(b2Fixture * pOther)
{
	if (CharacterBase* player = static_cast<CharacterBase*>(pOther->GetBody()->GetUserData()))
	{

		if (m_hideCharacter)
		{
			for (int y = 0; y < m_WidthHeight.y; ++y)
			{
				for (int x = 0; x < m_WidthHeight.x; ++x)
				{
					if (m_hideCharacter)
					{
						m_pPTiles[y*m_WidthHeight.x + x]->SetTextures(Tile::GetTexture(m_Tiletype, false));
					}
				}
			}
		}
	}

	m_hideCharacter = false;

}

void TiledObject::Draw()
{
	this->DrawTiledMap();
}
