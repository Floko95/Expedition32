using BitDuc.EnhancedTimeline.Timeline;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Observable
{
    /// @cond EXCLUDE
    [TrackClipType(typeof(ObservableClip))]
    [TrackBindingType(typeof(TimelineBus))]
    [TrackColor(1f, 0.4f, 0f)]
    internal class ObservableTrack : TrackAsset { }
}
