using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPather : Pather
{
    
    float peakHeight = 2f;
    public SmallPather(SmallPiece _piece)
    {
        piece = _piece;
        SnapToGrid();
    }
    public override void SnapToGrid() //with a raycast
    {
        //raycast downward with base cell... where did I put that? HexHelpers.CellUnderfoot
        HexCell cellUnderfoot = HexHelpers.CellUnderfoot(piece.hexBase.gameObject);
        if (cellUnderfoot is null) Debug.Log("Cell underfoot is null!", piece.gameObject);
        SnapToGrid(cellUnderfoot);
        SnapToFacing();
    }
    public override void SnapToGrid(HexCell hexCell)
    {
        //check if occupied
        if(hexCell.IsOccupied() && hexCell.Occupant() != piece)
        {
            //occupied. try neighbors. Will create error if can't find space...
            foreach(var n in hexCell.Neighbors())
            {
                if (n.IsOccupied()) continue;
                SnapToGrid(n);
                return;
            }
            Debug.Log("ERROR: too many pieces placed overlapping");
            return;
        }
        Vector3 cellPosition = hexCell.transform.position;
        cellPosition.y = piece.transform.position.y;
        piece.transform.position = cellPosition;
        piece.currentCell = hexCell;
        piece.currentCoord = hexCell.hexCoord;
        if(!SingletonManager.Instance.hexGridManager.pieceLocationLookup.ContainsValue(piece))
            SingletonManager.Instance.hexGridManager.pieceLocationLookup.Add(hexCell.hexCoord, piece);
    }
    public override void MoveTo(HexCell destination)
    {
        //Debug.Log($"MoveTo called. dest coords: {destination.hexCoord.i},{destination.hexCoord.j}", piece.gameObject);
        var path = PathTo(piece.currentCell, destination, piece.pieceStats.moveRange);
        SingletonManager.Instance.hexGridManager.pieceLocationLookup.Remove(piece.currentCoord);
        HexCell compromiseDestination = path[path.Count - 1];
        SingletonManager.Instance.hexGridManager.pieceLocationLookup.Add(compromiseDestination.hexCoord, piece);
        piece.StartCoroutine(FollowPath(path));
    }

    public override IEnumerator FollowPath(List<HexCell> path)
    {
        //Debug.Log($"pathCount: {path.Count}", piece.gameObject);
        float moveSpeed = piece.pieceStats.moveSpeed;
        int frames = 60;
        frames = (int)(frames / moveSpeed);
         Vector3 destination = path[path.Count - 1].transform.position;
        Vector3 origin = piece.currentCell.transform.position;
        Vector3 displacement = destination - origin;
        Quaternion startingRot = piece.transform.rotation;
        float deltaAngle = Vector3.SignedAngle(startingRot * Vector3.forward, displacement, Vector3.up);
        Quaternion endingRot = startingRot * Quaternion.AngleAxis(deltaAngle, Vector3.up);
        piece.StartCoroutine(FaceDirection(startingRot, endingRot, frames / 2));
        piece.facing = HexHelpers.FacingDirection(endingRot * Vector3.forward);
        foreach (var pathCell in path)
        {
            yield return ArcMoveToCell(pathCell, frames);
        }
        SnapToGrid();
        //Notify Piece that movement is finished
        piece.movementFinished = true;
        //Player checks this and tells GSM movement is finished

    }
    IEnumerator FaceDirection(Quaternion start, Quaternion end, int frames)
    {
        float delta = 1f / frames;
        for(int i = 1; i <= frames; i++)
        {
            yield return new WaitForEndOfFrame();
            float progress = delta * i;
            piece.transform.rotation = Quaternion.Lerp(start, end, progress);
            
        }
    }
    IEnumerator ArcMoveToCell(HexCell cell, int frames)
    {
        float delta = 1f / (frames);
        float peakHeight = piece.pieceStats.jumpHeight; //where to put this? put it with moveSpeed
        //lerp between start and end on flat ground
        Vector3 startingCell = piece.currentCell.transform.position;
        Vector3 endingCell = cell.transform.position;
        //add in y value after lerp
        for (int i = 1; i <= frames; i++)
        {
            yield return new WaitForEndOfFrame();
            float progress = i * delta;
            Vector3 pos = Vector3.Lerp(startingCell, endingCell, progress);
            pos.y = ArcHeight(progress);
            piece.transform.position = pos;
        }
        piece.currentCell = cell;
    }
    public override IEnumerator Hop()
    {
        int frames = 60;
        frames = (int)(frames / piece.pieceStats.moveSpeed);
        float delta = 1f / (frames);
        for (int i = 1; i <= frames; i++)
        {
            yield return new WaitForEndOfFrame();
            float progress = i * delta;
            Vector3 pos = piece.transform.position;
            pos.y = ArcHeight(progress);
            piece.transform.position = pos;
        }
        SnapToGrid();
        piece.movementFinished = true;
    }
    float ArcHeight(float x)
    {
        float y = -(x - 1f) * x * 4 * peakHeight;
        return y;
    }
    public override List<HexCell> PathTo(HexCell origin, HexCell destination, int maxSteps)
    {
        var path = new List<HexCell>();
        if (origin == destination)
        {
            //Debug.Log("error! pathing to same cell");
            path.Add(destination);
            return path;
        }
        var processed = new Dictionary<HexCell, int>();
        var queue = new Queue<(HexCell, int)>();        
        bool destinationReached = false;
        queue.Enqueue((origin, 0));
        while (queue.Count != 0) //if destination reached, breakout. If queue.Count == 0, destination not reached and return closest
        {
            var dq = queue.Dequeue();
            HexCell hC = dq.Item1;
            int steps = dq.Item2;
            if (processed.ContainsKey(hC)) continue;
            processed.Add(hC, steps);
            if (hC == destination)
            {
                destinationReached = true;
                break;
            }

            if (steps == maxSteps) continue; //destination will not be reached if it is outside of range
            foreach (var cell in hC.Neighbors())
            {
                if (cell.IsBlocked(piece.pieceStats.team)) continue;
                if (processed.ContainsKey(cell)) continue;
                queue.Enqueue((cell, steps + 1));
            }
        }

        
        if (!destinationReached ||
            (destination.IsOccupied() && destination.Occupant() != piece)) //why is destination not reached? occupied, blocked
        {
            //Debug.Log("Destination can't be reached! Compromising");
            //should return a path to the minDist
            HexCell compromiseDestination = null;
            int minDist = int.MaxValue;
            foreach(var entry in processed)
            {
                if (entry.Key.IsOccupied()) continue;
                int dist = entry.Key.hexCoord.DistanceFrom(destination.hexCoord);
                
                if(dist < minDist)
                {
                    minDist = dist;
                    compromiseDestination = entry.Key;
                    //Debug.Log($"distance from {entry.Key.hexCoord.i},{entry.Key.hexCoord.j} to {destination.hexCoord.i}, {destination.hexCoord.j}: {minDist}");
                }
            }
            //Debug.Log("compromising! dist from dest: " + minDist);
            return PathTo(origin, compromiseDestination, piece.pieceStats.moveRange);
        }
        else
        {
            path.Add(destination);
            int stepCount = processed[destination];
            HexCell currentStep = destination;
            while (stepCount != 1)
            {
                FindPreviousStep();
            }

            path.Reverse();
            //now you should have a path that goes 1,2,3,destination
            return path;

            void FindPreviousStep() //add to path
            {
                foreach (var item in currentStep.Neighbors())
                {
                    //find neighbor with step-value that is one lower
                    if (processed.ContainsKey(item) &&
                        processed[item] == stepCount - 1)
                    {
                        path.Add(item);
                        currentStep = item;
                        stepCount--;
                    }
                }
            }
        }
    }

    public int PathDistance(HexCell origin, HexCell destination) => PathTo(origin, destination, int.MaxValue).Count;
    
}
