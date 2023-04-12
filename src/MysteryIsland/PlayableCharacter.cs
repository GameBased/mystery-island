using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace MysteryIsland;

public class PlayableCharacter : ICollisionActor
{
    // the constants are defined in the .sf file
    const string ANIMATION_WALK_BACKWARD = "walk-backward";
    const string ANIMATION_WALK_FORWARD  = "walk-forward";
    const string ANIMATION_WALK_RIGHT    = "walk-right";
    const string ANIMATION_WALK_LEFT     = "walk-left";
    const string ANIMATION_DEFAULT       = "look-backward";

    private AnimatedSprite? sprite;
    // private RectangleF boundingRectangle;
    private Vector2 previousPosition = new Vector2(100, 100);
    private Vector2 _position = new Vector2(100, 100); // TODO: this should come from the map!
    public Vector2 Position => _position;

    public IShapeF Bounds => new RectangleF(
             x: _position.X - 10,
             y: _position.Y + 4,
         width: 22,
        height: 18);

    private string animation = ANIMATION_DEFAULT;

    public void LoadContent(ContentManager content)
    {
        var spriteSheet = content.Load<SpriteSheet>("characters/char1-sprite.sf", new JsonContentLoader());
        sprite = new AnimatedSprite(spriteSheet);
        // boundingRectangle = sprite.GetBoundingRectangle(new Transform2());
        sprite.Play("walk-backward");
    }


    public void Update(GameTime gameTime)
    {
        if (sprite is null) return;

        var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var walkSpeed = deltaSeconds * 128;
        var keyboard = KeyboardHelper.State;
        animation = animation.Replace("walk", "look", StringComparison.InvariantCulture);
        previousPosition = Position;

        if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
        {
            animation = ANIMATION_WALK_FORWARD;
            _position.Y -= walkSpeed;
        }

        if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
        {
            animation = ANIMATION_WALK_BACKWARD;
            _position.Y += walkSpeed;
        }

        if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        {
            animation = ANIMATION_WALK_LEFT;
            _position.X -= walkSpeed;
        }

        if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        {
            animation = ANIMATION_WALK_RIGHT;
            _position.X += walkSpeed;
        }

        sprite.Play(animation);
        sprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, Position);
    }

    public virtual void OnCollision(CollisionEventArgs collisionInfo)
    {
        _position = previousPosition;

    }
}
