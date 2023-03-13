// TileTypes.h
#pragma once

enum class TileType
{
    // normal tiles
    kGrass,    // easy to move on; low weight
    kForest,   // hard to move on; high weight
    kRiver,    // unable to pass

    kNormalTileTypeCount,  // not actually a valid type...

    // special tiles
    kNone,         // no overlay
    kStart,        // the start tile
    kPath,         // the path we found
    kOpenSet,      // the open set of tiles we still need to process
    kClosedSet,    // the closed set of tiles we've already processed
};
