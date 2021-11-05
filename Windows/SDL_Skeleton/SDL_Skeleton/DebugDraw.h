#pragma once

#include <Box2D.h>

class DebugDraw : public b2Draw
{
private:
	struct SDL_Renderer* m_pRenderer;
public:
	DebugDraw();
	~DebugDraw();

	void Init(struct SDL_Renderer* pRenderer) { m_pRenderer = pRenderer; }

	// Inherited via b2Draw
	virtual void DrawPolygon(const b2Vec2* vertices, int32 vertexCount, const b2Color& color) override;
	virtual void DrawSolidPolygon(const b2Vec2* vertices, int32 vertexCount, const b2Color& color) override;
	virtual void DrawCircle(const b2Vec2& center, float32 radius, const b2Color& color) override;
	virtual void DrawSolidCircle(const b2Vec2& center, float32 radius, const b2Vec2& axis, const b2Color& color) override;
	virtual void DrawSegment(const b2Vec2& p1, const b2Vec2& p2, const b2Color& color) override;
	virtual void DrawTransform(const b2Transform & xf) override;
	virtual void DrawPoint(const b2Vec2& p, float32 size, const b2Color& color) override;
};

