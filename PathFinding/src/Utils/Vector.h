// Vector.h
#pragma once

#include "Macros.h"


//---------------------------------------------------------------------------------------------------------------------
// 2D vector
//---------------------------------------------------------------------------------------------------------------------
class Vec2
{
public:
    float x, y;

    // construction
    Vec2(void) { x = y = 0; }
    Vec2(const Vec2& v2) { x = v2.x; y = v2.y; }
    Vec2(const float _x, const float _y) { x = _x; y = _y; }

    // primary operators
    Vec2& operator=(const Vec2& other) { x = other.x; y = other.y; return (*this); }
    Vec2& operator*=(float scalar) { x *= scalar; y *= scalar; return (*this); }
    bool operator==(const Vec2& other) { return (x == other.x && y == other.y); }
    bool operator!=(const Vec2& other) { return (x != other.x || y != other.y); }
    Vec2& operator+=(const Vec2& other) { x += other.x; y += other.y; return (*this); }
    Vec2& operator-=(const Vec2& other) { x -= other.x; y -= other.y; return (*this); }
    const Vec2 operator-() { return Vec2(-x, -y); }  // inverse of the vector

    // other math functions
    float Length(void) const;
    float LengthSquared(void) const { return ((x * x) + (y * y)); }
    void Normalize(void);
    bool IsExtremelyCloseTo(const Vec2& right) const;
};

inline Vec2 operator+(const Vec2& left, const Vec2& right)
{
    return (Vec2(left.x + right.x, left.y + right.y));
}

inline Vec2 operator-(const Vec2& left, const Vec2& right)
{
    return (Vec2(left.x - right.x, left.y - right.y));
}

inline Vec2 operator*(const Vec2& left, const Vec2& right)
{
    return (Vec2(left.x*right.x, left.y*right.y));
}

inline Vec2 operator*(const Vec2& left, const float right)
{
    return (Vec2(left.x*right, left.y*right));
}

inline Vec2 operator*(const float left, const Vec2& right)
{
    return (Vec2(left*right.x, left*right.y));
}

inline Vec2 operator/(const Vec2& left, const float right)
{
    return (Vec2(left.x/right, left.y/right));
}

