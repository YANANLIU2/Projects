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

    // The current goal for path finding
    Tile* m_pCurrentGoal;

    // A max-heap priority queue to store nodes: store here to split the work for multiple frames
    OpenSet openSet;

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
    inline Tile* GetTile(int index) { return m_pTiles[index]; }
    inline const int GetIndexFromGridPos(int x, int y) const { return (y * kWorldWidth) + x; }
};

