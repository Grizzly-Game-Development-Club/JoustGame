using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Waypoint : MonoBehaviour {

    private int waypointID;
    public WaypointType wayPointType;
    public List<Valid_Waypoint> validWaypointConnection;

    public int WaypointID
    {
        get { return waypointID; }
        set { waypointID = value; }
    }

    public WaypointType WayPointType
    {
        get { return wayPointType; }
        set { wayPointType = value; }
    }
    
    public List<Valid_Waypoint> ValidWaypointConnection
    {
        get { return validWaypointConnection; }
        set { validWaypointConnection = value; }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_Controller enemyScript = collision.GetComponent<Enemy_Controller>();


        if (enemyScript.nextWaypoint.transform == this.transform) {
            Debug.Log("Work");
            enemyScript.CurrentWaypoint = this.gameObject;
            enemyScript.EnemyState = EnemyState.ARRIVED; 
        }
    }


    
    public void OnDrawGizmosSelected()
    {
        
        foreach (Valid_Waypoint temp in validWaypointConnection) {
            //float arrowHeadWidth = .10f;

            //Gizmos.color = Color.blue;
            Vector2 start = AsVector2(this.GetComponent<Transform>());
            Vector2 end = AsVector2(temp.WaypointObject.GetComponent<Transform>());

            Gizmos.DrawLine(start, end);
            
        /*
        Vector2 middle = new Vector2(((start.x + end.x) / 2), ((start.y + end.y) / 2));
        float Angle = AngleBetweenVector2(end, start) + 360;



        Vector2 sideLineStart1 = new Vector2(start.x + arrowHeadWidth, start.y + arrowHeadWidth);
        Vector2 sideLineEnd1 = new Vector2(end.x + arrowHeadWidth, end.y + arrowHeadWidth);
        Vector2 sideLineMiddle1 = new Vector2(((sideLineStart1.x + sideLineEnd1.x) / 2), ((sideLineStart1.y + sideLineEnd1.y) / 2));
        float Slope1 = getSlope(sideLineStart1, sideLineEnd1);
        float BValue1 = getBValue(Slope1, sideLineStart1);




        Vector2 sideLineStart2 = new Vector2(start.x - arrowHeadWidth, start.y - arrowHeadWidth);
        Vector2 sideLineEnd2 = new Vector2(end.x - arrowHeadWidth, end.y - arrowHeadWidth);
        Vector2 sideLineMiddle2 = new Vector2(((sideLineStart2.x + sideLineEnd2.x) / 2), ((sideLineStart2.y + sideLineEnd2.y) / 2));
        float Slope2 = getSlope(sideLineStart2, sideLineEnd2);
        float BValue2 = getBValue(Slope2, sideLineStart2);



        Gizmos.color = Color.red;
        Gizmos.DrawLine(sideLineStart1, sideLineEnd1);
        Gizmos.DrawLine(sideLineStart2, sideLineEnd2);


        for (float incrumentValue = .01f; incrumentValue <= 1f; incrumentValue += .01f) {
            Debug.Log("IV: " + incrumentValue);
            Vector2 testValue = getTestValue(Angle, Slope1, BValue1, sideLineStart1, incrumentValue);
            float testAngle = AngleBetweenVector2(sideLineMiddle1, testValue);
            //Debug.Log(testValue + " " + temp.waypointObject.name);
            //Debug.Log("Acceptable between " + ((Angle1 - 5) + " & " + (Angle1 + 5) + " ; Test Value: " + testAngle));
            if (Angle - 5 <= testAngle && testAngle <= Angle + 5) {
                Gizmos.color = Color.green;
                Debug.Log("Drew");
                Gizmos.DrawLine(middle, testValue);
                break;
            }else if (incrumentValue >= .99f) {
                Debug.Log("---------------------------------------------------------------------");
                Gizmos.color = Color.green;
                Gizmos.DrawLine(middle, testValue);
            }              

        }

        Gizmos.color = Color.blue;


        incrumentValue = .5f;
        valueNotFound = true;
        while (valueNotFound)
        {
            Vector2 testValue = getTestValue(Angle2, Slope2, BValue2, sideLineEnd2, incrumentValue);
            float testAngle = AngleBetweenVector2(sideLineMiddle2, testValue);
            Debug.Log("Acceptable between " + ((Angle1 - 5) % 360) + " & " + ((Angle1 + 5) % 360) + " ; Test Value: " + testAngle);
            if ((Angle1 - 5) % 360 <= testAngle && testAngle <= (Angle1 + 5) % 360)
            {
                Gizmos.DrawLine(sideLineMiddle2, testValue);
                valueNotFound = true;

            }

            incrumentValue += .5f;
            if (incrumentValue == .50f)
            {
                valueNotFound = true;
            }
        }





        Gizmos.DrawLine(start, end);



        Gizmos.DrawLine(start, middle);
        Gizmos.color = Color.white;

        float reverseAngle = AngleBetweenVector2(end, start)/2;
        float arrowHeadLength = 1;
        float arrowHeadAngle = 80;

        float x1 = arrowHeadLength * Mathf.Cos((reverseAngle+20) % 360) + middle.x;
        float y1 = arrowHeadLength * Mathf.Sin((reverseAngle+20) % 360) + middle.x;

        Gizmos.DrawLine(middle, new Vector2(x1,y1));








        //L1
        float distanceBetweenStartAndEnd = Mathf.Sqrt(Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));
        //L2
        float arrowHeadLength = .50f;
        //A
        float arrowHeadAngle = 20f;



        end = AsVector2(temp.waypointObject.GetComponent<Transform>())/2;

        //X3
        float x3 = end.x + (arrowHeadLength / distanceBetweenStartAndEnd) * (((start.x - end.x)* Mathf.Cos(arrowHeadAngle)) + ((start.x - end.x) * Mathf.Sin(arrowHeadAngle)));
        //Y3
        float y3 = end.y + (arrowHeadLength / distanceBetweenStartAndEnd) * (((start.y - end.y) * Mathf.Cos(arrowHeadAngle)) - ((start.y - end.y) * Mathf.Sin(arrowHeadAngle)));
        //X4
        float x4 = end.x + (arrowHeadLength / distanceBetweenStartAndEnd) * (((start.x - end.x) * Mathf.Cos(arrowHeadAngle)) - ((start.x - end.x) * Mathf.Sin(arrowHeadAngle)));
        //Y4
        float y4 = end.y + (arrowHeadLength / distanceBetweenStartAndEnd) * (((start.y - end.y) * Mathf.Cos(arrowHeadAngle)) + ((start.y - end.y) * Mathf.Sin(arrowHeadAngle)));

        Vector2 leftArrow = new Vector2(x3,y3);
        Vector2 rightArrow = new Vector2(x4, y4);


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(end, leftArrow);
        Gizmos.DrawLine(end, rightArrow);




        //float angle = AngleBetweenVector2(start, end);
        float reverseAngle = AngleBetweenVector2(end, start);






        //Debug.Log(angle + " to " + temp.waypointObject.name);


        float rightAngle = reverseAngle + arrowHeadAngle;
        float leftAngle = reverseAngle - arrowHeadAngle;
        //Debug.Log(reverseAngle + " + " + arrowHeadAngle + " = " + NewDir);

        Vector2 NewPos = new Vector2(arrowHeadLength * Mathf.Sin(rightAngle), arrowHeadLength * Mathf.Cos(rightAngle));
        Vector2 NewWorldPos = NewPos + end;

        Vector2 leftArrow = new Vector2(Mathf.Cos(reverseAngle) + end.x , Mathf.Sin(reverseAngle) + end.y);
        //Debug.Log("Start X: " + end.x + " Y: " + end.y + " Temp: " + temp.waypointObject.name);
        //Debug.Log("X: " + leftArrow.x + " Y: " + leftArrow.y);
        //Debug.Log(temp.waypointObject.name + " to " + this.name + " = " + AngleBetweenVector2(end,start));


    */
        }
        
    }

    private Vector2 AsVector2(Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

}

    /*
    IEnumerator HoldOneSecond()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(20f);
    }

        private Vector2 AsVector2(Transform transform) {
        return new Vector2(transform.position.x,transform.position.y);
    }

    private float getSlope(Vector2 start, Vector2 end) {
        return ((end.y - start.y) / (end.x - start.x));

    }

    private float getBValue(float slope, Vector2 start)
    {
        return start.y - (start.x * slope);

    }
    private float findYValue(float m, float b, float x) {
        return (m * x) + b;
    }
    private float findXValue(float m, float b, float y)
    {
        return (y - b) / m;

    }

    private Vector2 getTestValue(float Angle, float slope, float b, Vector2 sideLineEnd, float incrumentValue) {
        //+X
        if (405 <= Angle && Angle <= 495)
        {
            float newX = sideLineEnd.x + incrumentValue;
            float newY = findYValue(slope, b, newX);
            return new Vector2(newX, newY);
        }
        //-Y
        else if (495 <= Angle && Angle <= 585)
        {
            float newY = sideLineEnd.y + incrumentValue;
            float newX = findXValue(slope, b, sideLineEnd.y - incrumentValue);
            return new Vector2(newX, newY);
        }
        //-X
        else if (585 <= Angle && Angle <= 675)
        {
            float newX = sideLineEnd.x + incrumentValue;
            float newY = findYValue(slope, b, sideLineEnd.x - incrumentValue);
            return new Vector2(newX, newY);
        }
        //+Y
        else if (675 <= Angle && Angle <= 720 || 360 <= Angle && Angle <= 405)
        {
            float newY = sideLineEnd.y + incrumentValue;
            float newX = findXValue(slope, b, sideLineEnd.y + incrumentValue);
            return new Vector2(newX, newY);
        }
        else {
            return new Vector2(0,0);
        }

    }

    float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        float x = vec2.x - vec1.x;
        float y = vec2.y - vec1.y;
        float angle = Mathf.Atan2(y, x);

        //If negative angle, turn it into positive
        if ((angle * Mathf.Rad2Deg) < 0)
        {
            return (angle* Mathf.Rad2Deg) + 360;
        }
        else {
           return (angle* Mathf.Rad2Deg);
        }
    }
    */

