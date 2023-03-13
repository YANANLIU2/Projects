// Random.h
#pragma once

namespace Random
{
    void Seed();
    float FRand();  // returns a random number from 0 - 1 [inclusive]
    float SignedFRand();  // returns a random number from -1 to 1 [inclusive]
    int RangeRand(int min, int max); // pick a random number from min to max [inclusive]
}

