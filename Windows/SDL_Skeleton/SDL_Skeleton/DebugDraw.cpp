
#include <SDL_render.h>
#include "Defines.h"
#include "DebugDraw.h"

DebugDraw::DebugDraw()
{
}


DebugDraw::~DebugDraw()
{
}

void DebugDraw::DrawPolygon(const b2Vec2* vertices, int32 vertexCount, const b2Color& color)
{
}

void DebugDraw::DrawSolidPolygon(const b2Vec2* vertices, int32 vertexCount, const b2Color& color)
{
	SDL_Point* pPoints = new SDL_Point[vertexCount + 1];

	for (int vertIndex = 0; vertIndex < vertexCount + 1; ++vertIndex)
	{
		pPoints[vertIndex].x = int(vertices[vertIndex % vertexCount].x * kPixelsPerMeter);
		pPoints[vertIndex].y = int(vertices[vertIndex % vertexCount].y * kPixelsPerMeter);
	}
	
	SDL_SetRenderDrawColor(m_pRenderer, color.r * 255, color.g * 255, color.b * 255, color.a * 255);
	SDL_RenderDrawLines(m_pRenderer, pPoints, vertexCount + 1);

	delete[] pPoints;
}

void DebugDraw::DrawCircle(const b2Vec2& center, float32 radius, const b2Color& color)
{
}

void DebugDraw::DrawSolidCircle(const b2Vec2& center, float32 radius, const b2Vec2& axis, const b2Color& color)
{
}

void DebugDraw::DrawSegment(const b2Vec2& p1, const b2Vec2& p2, const b2Color& color)
{
	SDL_SetRenderDrawColor(m_pRenderer, color.r * 255, color.g * 255, color.b * 255, color.a * 255);
	SDL_RenderDrawLine(m_pRenderer, p1.x * kPixelsPerMeter, p1.y * kPixelsPerMeter, p2.x * kPixelsPerMeter, p2.y * kPixelsPerMeter);
}

void DebugDraw::DrawTransform(const b2Transform& xf)
{
}

void DebugDraw::DrawPoint(const b2Vec2& p, float32 size, const b2Color& color)
{
}
