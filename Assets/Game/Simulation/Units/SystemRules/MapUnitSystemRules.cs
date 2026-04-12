
public class MapUnitSystemRules
{
    //interact with the unitGameState

    //unit initialize movement to somewhere
    //if ennemy/hostile unit initialize battle

    //unit halte

    public DataRegistery InitializeMovement(int unitId, int destination, DataRegistery _registery)
    {
        //call pathFinder
        //
        return _registery;
    }
    public DataRegistery Move(int unitId,DataRegistery _registery)
    {
        //resolve movement
        MapUnit unit = _registery.mapUnitDict[unitId];
        
        //if hostile in targeted tile and not in battle initialize battle
        return _registery;
    }
    public DataRegistery Halt(int unitId,DataRegistery _registery)
    {
        MapUnit unit =_registery.mapUnitDict[unitId];

        unit.Halt();
        return _registery;
    }

    public DataRegistery DestinationReached(int unitId,DataRegistery _registery)
    {
        MapUnit unit = _registery.mapUnitDict[unitId];


        _registery.mapUnitState[unit.currentTileID].Remove(unit);
        _registery.mapUnitState[unit.targetTileId].Add(unit);

        unit.currentTileID = unit.targetTileId;

        if (unit.queue.Count > 0) 
        { 
            unit.targetTileId = unit.queue.Dequeue();
            //call move
        }
        else
        {
            unit.isMoving = false;
            unit.targetTileId = -1;
        }
        return _registery;
    }

    public DataRegistery CreateUnit(MapUnit unit,DataRegistery _registery)
    {
        _registery.mapUnitDict.Add(unit.Id, unit);
        _registery.mapUnitState[unit.currentTileID].Add(unit);
        return _registery;
    }

    public DataRegistery DeleteUnit(MapUnit unit, DataRegistery _registery)
    {
        _registery.mapUnitDict.Remove(unit.Id);
        _registery.mapUnitState[unit.currentTileID].Remove(unit);
        return _registery;
    }


    //unit move out of battle

    //unit create
    //unit delete


    //interface unity to this


}
