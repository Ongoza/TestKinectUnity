using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class AnimSkeleton 
{
    // animmation as list of data
    public List<AnimSkeletonItem> anim = new List<AnimSkeletonItem>();

}
 
[System.Serializable] public class AnimSkeletonItem
{
    // One frame skeleton position
    public float[] data;
    public int[] state;
}