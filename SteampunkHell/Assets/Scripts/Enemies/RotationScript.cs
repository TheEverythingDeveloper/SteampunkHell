using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    /// <summary>
    /// Rotar el vector que le pasemos. Basicamente vector3(0,180,0) va a ser darse vuelta en y por ejemplo.
    /// </summary>
    /// <param name="rotation"> La rotacion que va a hacer </param>
    /// <param name="duration"> El tiempo que va a tardar en rotar </param>
    /// <returns></returns>
    protected IEnumerator RotateAround(Vector3 rotation, float duration)
    {
        float t = 0;
        Quaternion initialRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(rotation).eulerAngles.y, 0));
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, t);
            yield return new WaitForEndOfFrame();
        }
    }
    /// <summary>
    /// Va a rotar un objeto 
    /// </summary>
    /// <param name="finalLookAt"> Hacia donde va a hacer la rotacion </param>
    /// <param name="duration"> Cuanto va a tardar en rotar hasta mirar a finalLookAt </param>
    /// <returns></returns>
    protected IEnumerator RotateAroundTo(Vector3 finalLookAt, float duration)
    {
        float t = 0;
        Quaternion initialRotation = transform.rotation;
        Vector3 direction = finalLookAt - transform.position;
        float finalAngle = Vector3.Angle(transform.forward, direction);
        /*Vector3 finalAngleVector = new Vector3(0, finalAngle, 0);
        Quaternion finalRotation = transform.rotation * Quaternion.Euler(finalAngleVector);*/
        Quaternion finalRotation = Quaternion.Euler(new Vector3(0,Quaternion.LookRotation(direction).eulerAngles.y,0));

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, t);
            yield return new WaitForEndOfFrame();
        }
    }
}