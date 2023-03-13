// Tile.h
#pragma once

#include "SDL.h"
#include "TileTypes.h"

#include <float.h>
#include <limits>

class Vec2;
class TileRenderer;
class Tile
{
	int m_tile;
	float m_costSoFar;
	Tile* m_pPrev;
	bool m_bClosed;

public:
    static constexpr int kTileWidth = 16;
    static constexpr int kTileHeight = 16;

private:
    // Tile renderers; one for the base layer (the entire tile) and the other for the overlay (a smaller box over 
    // the tile.  The base layer represents the terrain while the overlay is used to show pathing information.
    // Note that the base layer cannot be nullptr, but the overlay can.
    TileRenderer* m_pBaseLayer;
    TileRenderer* m_pOverlay;

    // Tile size/position information used for rendering.
    SDL_Rect m_baseRect;
    SDL_Rect m_overlayRect;

    // A number representing how difficult it is to move through this tile.  The edge weight between two adjacent 
    // tiles is calculated as the average of the weights of the two tiles.  Note that a weight of 0 means the tile 
    // is impassible.
    float m_weight;

public:
    Tile(int x, int y, float weight, int tile);
    ~Tile();

    float get_weight() const { return m_weight; }

    void Render(SDL_Renderer* pRenderer);
    void SetBaseLayer(TileType type);
    void SetOverlay(TileType type);

    Vec2 GetCenter() const;
    bool IsWalkable() const { return m_weight > 0; }
    bool IsGoal() const { return (m_pOverlay != nullptr); }

	void ResetPathFinding() { m_costSoFar = std::numeric_limits<float>::max(); m_pPrev = nullptr; m_bClosed = false; }

	// pathfinding
	// getters
	float GetCost() const { return m_costSoFar; }
	Tile* GetPrev() const { return m_pPrev; }
	bool IsClosed() const { return m_bClosed; }
	int GetTile() const { return m_tile; }

	// setters
	void SetCost(float cost) { m_costSoFar = cost; }
	void SetPrev(Tile* pPrev) { m_pPrev = pPrev; }
	void SetClosed() { m_bClosed = true; }
};
