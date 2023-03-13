// PathPlan.h
#pragma once

#include "../Utils/Vector.h"
#include <vector>
#include <queue>
#include <functional>
typedef std::vector<Vec2> Plan; //change plan from list to vector 

// open set type used to store all path finding nodes

class Tile;

typedef std::priority_queue<Tile*, std::vector<Tile*>, std::function< bool(Tile*, Tile*) >> OpenSet;