#pragma once
static constexpr int kNumGameObjects = 1000; // cannot handle 10000 yet cause some will starve
static constexpr double kPathFindingComputingTimePerFrame = 16.667; // in miliseconds