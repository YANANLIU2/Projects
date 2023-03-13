//  Standard C++ headers
#include <assert.h>
#include <stdlib.h>
#include <set>
#include <list>
#include <fstream>
#include <string>
#include <algorithm>
#include <cmath>        // std::abs

// Project
#include "World.h"
#include "../Application/Constants.h"
#include "../Utils/Macros.h"
#include "../Utils/Random.h"
#include "Tile.h"
#include "TileTypes.h"
#include "TileFactory.h"

//---------------------------------------------------------------------------------------------------------------------
// Constructor.
//---------------------------------------------------------------------------------------------------------------------
World::World()
    : openSet([](Tile* pLeft, Tile* pRight) {return pLeft->GetCost() > pRight->GetCost(); })
{
    memset(m_pTiles, 0, sizeof(Tile*) * kNumTiles);
}

//---------------------------------------------------------------------------------------------------------------------
// Destructor.
//---------------------------------------------------------------------------------------------------------------------
World::~World()
{
    DestroyWorld();
}

//---------------------------------------------------------------------------------------------------------------------
// Initializes the world by loading the map file.
//      -filename:  The map filename.
//      -return:    true if successful, false if not.
//---------------------------------------------------------------------------------------------------------------------
bool World::Init(const char* filename)
{
    DestroyWorld();

    // load the data file
    std::ifstream file;
    file.open(filename, std::ios::in);
    if (!file.is_open())
    {
        SDL_LogError(SDL_LOG_CATEGORY_ERROR, "Couldn't open world file.");
        return false;
    }

    // read the file
    std::vector<std::string> rawLines;
    rawLines.reserve(kWorldHeight);
    std::string temp;
    while (std::getline(file, temp))
    {
        rawLines.emplace_back(temp);
    }
    file.close();

    int y = 0;
    for (const std::string& line : rawLines)
    {
        int x = 0;
        for (char tile : line)
        {
            // get the tile type
			int index = GetIndexFromGridPos(x, y); //get index
            TileType type = TileType::kNone;
            switch (tile)
            {
                case '.': type = TileType::kGrass; break;
				case '#': type = TileType::kRiver; break;
                case '|': type = TileType::kForest; break;
				case '@': type = TileType::kGrass; m_pGoals.push_back(index); break; //find goal, push to vector

                default: SDL_LogError(SDL_LOG_CATEGORY_ERROR, "Invalid tile type"); break;
            }

            // create the tile
            m_pTiles[index] = TileFactory::CreateTile(x, y, type, index);
            ++x;

            // if this is a target tile, set the overlay
            if (tile == '@')
            {
                m_pTiles[index]->SetOverlay(TileType::kClosedSet);
            }
        }

        ++y;
    }

    return true;
}

//---------------------------------------------------------------------------------------------------------------------
// Destroys the world.  Call Init() to reinitialize it.
//---------------------------------------------------------------------------------------------------------------------
void World::DestroyWorld()
{
    for (int i = 0; i < kNumTiles; ++i)
    {
        SAFE_DELETE(m_pTiles[i]);
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Called every frame to render the world.
//      -pRenderer: The SDL renderer to use.
//---------------------------------------------------------------------------------------------------------------------
void World::Render(SDL_Renderer* pRenderer)
{
    for (int i = 0; i < kNumTiles; ++i)
    {
        if (m_pTiles[i])
        {
            m_pTiles[i]->Render(pRenderer);
        }
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Generates a path to the "best" target.
//      -startPos:  The world position to start from.
//      -return:    The plan to get to the chosen goal.
//---------------------------------------------------------------------------------------------------------------------
void World::GeneratePathToBestTarget(const Vec2 startPos, Plan& path)
{
    // If open set is empty, then the last game object has been processed. Set the goal for the new game object
    if (openSet.empty())
    {
        // Calculate the goal
        int startIndex = GetTileIndexFromWorldPos(startPos.x, startPos.y);
        m_pCurrentGoal = m_pTiles[GetARandGoalTile(RoundToNearest(startPos.x / Tile::kTileWidth), RoundToNearest(startPos.y / Tile::kTileHeight))];

        // Create the start node and add it to the planningNodes
        Tile* pStartTile = m_pTiles[startIndex];
        if (pStartTile)
        {
            ResetTiles();
            // Add the start vertex
            openSet.push(pStartTile);
            pStartTile->SetCost(0);
        }
    }

    Tile* pNode = nullptr;
    // Keep going as long as there's anything in the open set
    while (!openSet.empty())
    {
        // Grab the top of the open set: the highest priority tile
        pNode = openSet.top();
        if (pNode == m_pCurrentGoal) // If the node is goal, end the loop
        {
            break;
        }
        openSet.pop();

        // Add this vertex to the closed set.
        pNode->SetClosed();

        // Get all the neighbors of this node
        const int kNodeMaxNeighbors = 4;
        int outNeighbors[kNodeMaxNeighbors] = { -1 };
        int index = pNode->GetTile();
        int y = index / kWorldWidth;
        int x = index % kWorldWidth;

        // Left neighbor
        if (x - 1 >= 0)
        {
            outNeighbors[0] = GetIndexFromGridPos(x - 1, y);
        }

        // Right neighbor
        if (x + 1 < kWorldWidth)
        {
            outNeighbors[1] = GetIndexFromGridPos(x + 1, y);
        }

        // Bottom
        if (y - 1 >= 0)
        {
            outNeighbors[2] = GetIndexFromGridPos(x, y - 1);
        }

        // Top
        if (y + 1 < kWorldHeight)
        {
            outNeighbors[3] = GetIndexFromGridPos(x, y + 1);
        }

        // For each neighbor
        for (int neighborIndex : outNeighbors)
        {
            if (neighborIndex < 0 || neighborIndex >= kNumTiles)
            {
                continue;
            }
                
            if (m_pTiles[neighborIndex]->IsClosed())
            {
                continue;
            }
            // If neighborIndex = -1, means the neighbor does not exist, skip. If cost = 0, the neighbor is impassible
            if (neighborIndex < 0 || m_pTiles[neighborIndex]->get_weight() == 0)
            {
                continue;
            }
            // Relax the node. If this path is better, we insert it into the open set.
            if (Relax(pNode, m_pTiles[neighborIndex], GetWeight(pNode->GetTile(), m_pTiles[neighborIndex]->GetTile())))
            {
                openSet.push(m_pTiles[neighborIndex]);
            }
        }
    }

    // Build the path to the chosen goal if we find the goal 
    if (pNode == m_pCurrentGoal)
    {
        if (!openSet.empty())
        {
            while (m_pCurrentGoal)
            {
                Vec2 positionPair = m_pTiles[m_pCurrentGoal->GetTile()]->GetCenter();
                path.push_back(positionPair);
                m_pCurrentGoal = m_pCurrentGoal->GetPrev();
            }
        }
    }
}


//---------------------------------------------------------------------------------------------------------------------
// Reset all tiles to be ready for next path-finding
//---------------------------------------------------------------------------------------------------------------------
void World::ResetTiles() const
{
    for (int i = 0; i < kNumTiles; ++i)
    {
        if (m_pTiles[i] != nullptr)
        {
            m_pTiles[i]->ResetPathFinding();
        }
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Relaxes a node in the GeneratePathToBestTarget() search function.
//      -pSource:   The source node.
//      -pDest:     The destination node.
//      -weight:    The cost between the source and destination.
//      -return:    true if this path is better, false if not.
//---------------------------------------------------------------------------------------------------------------------
bool World::Relax(Tile* pSource, Tile* pDest, float weight) const
{
    if (pDest->GetCost() > pSource->GetCost() + weight)
    {
        pDest->SetCost(pSource->GetCost() + weight);
        pDest->SetPrev(pSource);
        return true;
    }
    return false;
}

//---------------------------------------------------------------------------------------------------------------------
// Gets the cost to go from one tile to another.
//      -source:    The index of the source tile.
//      -dest:      The index of the destination tile.
//      -return:    The cost.
//---------------------------------------------------------------------------------------------------------------------
float World::GetWeight(int source, int dest) const
{
    float sourceCost = m_pTiles[source]->get_weight();
    float destCost = m_pTiles[dest]->get_weight();
    float totalCost = sourceCost + destCost;
    return totalCost;
}

//---------------------------------------------------------------------------------------------------------------------
// Pick a random and non-minimum goal
//      -return:    The goal's index
//---------------------------------------------------------------------------------------------------------------------
int World::GetARandGoalTile(int x, int y) const
{
    // Find minimum goal
    int minDist = std::numeric_limits<int>::max();
    int minIndex = 0;
    for (int i = 0; i < m_pGoals.size(); ++i)
    {
        int index = m_pGoals[i];
        int goalX = m_pTiles[index]->GetTile() % kWorldWidth;
        int goalY = m_pTiles[index]->GetTile() / kWorldWidth;
        int distance = std::abs(goalX - x) + std::abs(goalY - y);
        if (distance < minDist)
        {
            minDist = distance;
            minIndex = i;
        }
    }

    // Pick A random except the minimum
    int rand = Random::RangeRand(0, int(m_pGoals.size() - 1));
    if (rand >= minIndex)
    {
        rand++;
        if (rand == m_pGoals.size())
        {
            rand = 0;
        }
    }
    return m_pGoals[rand];
}

//---------------------------------------------------------------------------------------------------------------------
// Returns a random tile position in world space.
//      -return:    The tile position.
//---------------------------------------------------------------------------------------------------------------------
int World::GetRandWalkableTile() const
{
    static constexpr int kThreshold = 10;

    for (int i = 0; i < kThreshold; ++i)
    {
        int index = Random::RangeRand(0, kNumTiles - 1);
        if (m_pTiles[index]->IsWalkable())
        {
            return index;
        }
    }

    return 0;
}

//---------------------------------------------------------------------------------------------------------------------
// Gets the tile index of the tile from a world position.
//      -x:         The world X.
//      -y:         The world Y.
//      -return:    The index of the tile at this position.
//---------------------------------------------------------------------------------------------------------------------
int World::GetTileIndexFromWorldPos(float x, float y) const
{
	int gridY = RoundToNearest(y / Tile::kTileHeight);
	int gridX = RoundToNearest(x / Tile::kTileWidth);
	return GetIndexFromGridPos(gridX, gridY);
}