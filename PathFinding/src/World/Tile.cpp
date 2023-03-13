// Tile.cpp
#include "Tile.h"
#include "TileRenderer.h"
#include "TileFactory.h"
#include "../Utils/Macros.h"
#include "../Utils/Vector.h"
#include <assert.h>

//---------------------------------------------------------------------------------------------------------------------
// Constructor.
//---------------------------------------------------------------------------------------------------------------------
Tile::Tile(int x, int y, float weight, int tile)
    : m_pBaseLayer(nullptr)
    , m_pOverlay(nullptr)
    , m_weight(weight)
	, m_tile(tile)
	, m_costSoFar(std::numeric_limits<float>::max())
	, m_pPrev(nullptr)
	, m_bClosed(false)
{
    // base rect
    m_baseRect.x = x;
    m_baseRect.y = y;
    m_baseRect.w = kTileWidth - 1;

    m_baseRect.h = kTileHeight - 1;

    // overlay rect
    m_overlayRect.x = m_baseRect.x + (kTileWidth / 4);
    m_overlayRect.y = m_baseRect.y + (kTileHeight / 4);
    m_overlayRect.w = kTileWidth / 2;
    m_overlayRect.h = kTileHeight / 2;
}

//---------------------------------------------------------------------------------------------------------------------
// Destructor.
//---------------------------------------------------------------------------------------------------------------------
Tile::~Tile()
{
    SAFE_DELETE(m_pOverlay);
    SAFE_DELETE(m_pBaseLayer);
}

//---------------------------------------------------------------------------------------------------------------------
// Renders a single tile by telling both of its layers to render.
//     -pRenderer: The SDL renderer to use.
//---------------------------------------------------------------------------------------------------------------------
void Tile::Render(SDL_Renderer* pRenderer)
{
    // no base layer
    if (!m_pBaseLayer)
    {
        SDL_LogError(SDL_LOG_CATEGORY_APPLICATION, "No base layer on tile.");
        return;
    }

    // render the base layer
    m_pBaseLayer->Render(pRenderer, m_baseRect);

    // render the overlay
    if (m_pOverlay)
    {
        m_pOverlay->Render(pRenderer, m_overlayRect);
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Sets the base layer for this tile.
//     -type: The new base layer type.
//---------------------------------------------------------------------------------------------------------------------
void Tile::SetBaseLayer(TileType type)
{
    assert(type != TileType::kNone);

    if (m_pBaseLayer)
    {
        SAFE_DELETE(m_pBaseLayer);
    }

    m_pBaseLayer = TileFactory::CreateTileRenderer(type);
}

//---------------------------------------------------------------------------------------------------------------------
// Sets the overlay for this tile.
//     -type: The new overlay type.
//---------------------------------------------------------------------------------------------------------------------
void Tile::SetOverlay(TileType type)
{
    if (m_pOverlay)
    {
        SAFE_DELETE(m_pOverlay);
    }

    // no overlay
    if (type != TileType::kNone)
    {
        m_pOverlay = TileFactory::CreateTileRenderer(type);
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Gets the pixel position of the center of the tile.
//      -return:    The world position of the center of the tile.
//---------------------------------------------------------------------------------------------------------------------
Vec2 Tile::GetCenter() const
{
    return Vec2(m_baseRect.x + (m_baseRect.w / 2.f), m_baseRect.y + (m_baseRect.h / 2.f));
}

