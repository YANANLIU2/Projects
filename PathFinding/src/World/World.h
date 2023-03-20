#pragma once

// STD C++ headers
#include <vector>
#include <queue> // priority_queue
#include <functional> // to wrap a lambda function and store it as a class variable

// SDL
#include <SDL.h>

// Project 
#include "../World/PathPlan.h"

class Tile;
class Vec2;
class World
{
    static constexpr int kWorldWidth = 60;
    static constexpr int kWorldHeight = 60;
    static constexpr size_t kNumTiles = kWorldWidth * kWorldHeight;

private:
    // Tile grid
    Tile* m_pTiles[kNumTiles];

	// Goal tiles 
	std::vector<int> m_pGoals;

public:
    // Constructor
    World();

    // Destructor
    ~World();

    // Init the world from a file
    bool Init(const char* filename);

    void DestroyWorld();

    void Render(SDL_Renderer* pRenderer);

    // PathFinding
    void GeneratePathToBestTarget(const Vec2 startPos, Plan& path);

private:
    void ResetTiles() const;
    bool Relax(Tile* pSource, Tile* pDest, float weight) const;
	float GetWeight(int source, int dest) const;
    int GetARandGoalTile(int x, int y) const;

    // Helper functions
public:
    int GetRandWalkableTile() const;
    int GetTileIndexFromWorldPos(float x, float y) const;
    bool GetGridPosFromWorldPos(float x, float y, int& outX, int& outY);
    Tile* GetTileFromGridPos(int x, int y);
    inline Tile* GetTile(int index) { return m_pTiles[index]; }
    inline const int GetIndexFromGridPos(int x, int y) const { return (y * kWorldWidth) + x; }
    inline bool IsTileIndexValid(int index) const { return index >= 0 && index < kNumTiles; }
};

