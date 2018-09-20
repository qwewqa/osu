// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using OpenTK;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModFlip : Mod, IApplicableToDrawableHitObjects
    {
        public override string Name => "Flip";
        public override string ShortenedName => "FP";
        public override FontAwesome Icon => FontAwesome.fa_arrows_h;
        public override ModType Type => ModType.Fun;
        public override string Description => "Circles flip around. No approach circles.";
        public override double ScoreMultiplier => 1;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            foreach (var drawable in drawables)
            {
                drawable.ApplyCustomUpdateState += ApplyBounceState;
            }
        }

        protected void ApplyBounceState(DrawableHitObject drawable, ArmedState state)
        {
            if (!(drawable is DrawableOsuHitObject)) return;
            if (state != ArmedState.Idle) return;

            var h = (OsuHitObject)drawable.HitObject;
            // +/- 1 so that these appear right
            var appearTime = h.StartTime - h.TimePreempt -1;
            var moveDuration = h.TimePreempt + 1;

            switch (drawable)
            {
                case DrawableHitCircle circle:
                    using (circle.BeginAbsoluteSequence(appearTime, true))
                    {
                        var origScale = drawable.Scale;

                        circle
                            .ScaleTo(origScale * new Vector2(-1.0f, 1.0f))
                            .ScaleTo(origScale, moveDuration, Easing.InOutSine);
                    }

                    circle.ApproachCircle.Hide();

                    break;

                case DrawableSlider slider:
                    using (slider.BeginAbsoluteSequence(appearTime, true))
                    {
                        var origScale = slider.Scale;

                        slider
                            .ScaleTo(origScale * new Vector2(-1.0f, 1.0f))
                            .ScaleTo(origScale, moveDuration, Easing.InOutSine);
                    }

                    break;
            }
        }
    }
}
