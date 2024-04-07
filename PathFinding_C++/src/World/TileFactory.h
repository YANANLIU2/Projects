// TileFactory.h
#pragma once

#include "TileTypes.h"

class Tile;
class TileRenderer;

//---------------------------------------------------------------------------------------------------------------------
// Static factory for creating tiles and tile renderers.
//---------------------------------------------------------------------------------------------------------------------
class TileFactory
{
public:
    static Tile* CreateTile(int gridX, int gridY, TileType baseLayer, int index, TileType overlay = TileType::kNone);
    static TileRenderer* CreateTileRenderer(TileType type);

private:
    static float GetWeightFromType(TileType type);
};
