#pragma once
static constexpr int kNumGameObjects = 1000; // cannot handle 10000 in debug x64 yet cause some will starve. release 100,000
static constexpr double kPathFindingComputingTimePerFrame = 16.667; // in miliseconds