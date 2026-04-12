using System.Collections.Generic;

public class MapUnit
{
    public int Id;
    public int currentTileID;
    public int targetTileId;
    public int distanceLeft;
    public bool isMoving;
    public bool isInBattle;
    public Queue<int> queue = new Queue<int>();
    List<>

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
