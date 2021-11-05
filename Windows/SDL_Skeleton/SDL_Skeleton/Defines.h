#pragma once
#ifndef DEFINES_H
#define DEFINES_H

enum EMove
{
	None,

	Right,
	Left,
	Up,
	Down,

	StopRight,
	StopLeft,
	StopUp,
	StopDown,

	NUM_dir
};

#define WINDOW_WIDTH 1280
#define WINDOW_HEIGHT 640
#define GameName "Lingering Realm"
#define WindowsFont_Width 50
#define WindiowsFont_Height 50
#define SDL_CHECK(x) if(x == nullptr) { std::cout << "Error: " << SDL_GetError() << '\n'; }

constexpr int k_tilesize = 32;
constexpr float kPixelsPerMeter = 100;

struct Vector2
{
	float x;
	float y;
};

struct intpair
{
	int x;
	int y;
};

enum ETileType
{
	kFloor,
	kTree,
	kYellowFlower,
	kOrangeMushroom,
	kDefault,

	Num_tiletypes

};

enum EGameObjectType
{
	kPlayer,
	kTile,
	kBlock
};

#define ReadElementValue(parent, elementName, value, type) \
{ \
	XMLElement* mpElement = parent->FirstChildElement(elementName); \
	if(mpElement == nullptr) \
	{ \
		std::cout << "Could not find element: " << elementName << '\n'; \
	} \
\
	XMLError queryResult = mpElement->Query##type##Text(&value); \
	if(queryResult != XMLError::XML_SUCCESS) \
	{ \
		std::cout << "Failed to parse element: " << elementName << '\n'; \
	} \
}

#endif