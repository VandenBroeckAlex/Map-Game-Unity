using System.Collections.Generic;

public class UnitNavigation
{
    public int id;
    public int currentTileID;
    public int targetTileId;
    public int distanceLeft;
    public bool isMoving;
    public bool isInBattle;
    public bool land;
    public bool sea;
    public bool air;
    public Queue<int> queue = new Queue<int>();

    public UnitNavigation(int id,int currentTileId, bool land, bool sea, bool air)
    {
        this.id = id;
        this.currentTileID = currentTileId;
        this.targetTileId = -1;
        this.distanceLeft = 0;
        this.isMoving = false;
        this.isInBattle = false;
        this.land = land;
        this.sea = sea;
        this.air = air;
        
        
 
    }

    public void Halt()
    {
        if (isMoving)
        {
            this.targetTileId = -1;
            this.distanceLeft = 0;
            this.queue.Clear();
            this.isMoving = false;
        }
    }

    public void Move(int target,int distance,Queue<int> queue)
    {
        this.targetTileId = target;
        this.distanceLeft = distance;
        this.isMoving = true;
        this.queue = queue;
    }
}
