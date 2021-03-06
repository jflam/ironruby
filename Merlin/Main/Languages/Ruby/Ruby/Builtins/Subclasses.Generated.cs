﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Microsoft Public License, please send an email to 
 * ironruby@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using IronRuby.Runtime;
using Microsoft.Scripting.Utils;
using IronRuby.Compiler.Generation;
using System.Diagnostics;

namespace IronRuby.Builtins {
#if GENERATOR
    Stateless = ['MatchData', 'Proc', 'Range', 'RubyRegex']

    def generate
      Stateless.each do |cls| 
        @class = cls
        super 
      end
    end

    def Class *a
      @class
    end
#else
    public partial class /*$Class{*/Hash/*}*/ {
        public sealed partial class Subclass : /*$Class{*/Hash/*}*/, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public bool IsFrozen {
                get { return _instanceData != null && _instanceData.Frozen; }
            }

            public bool IsTainted {
                get { return _instanceData != null && _instanceData.Tainted; }
                set { GetInstanceData().Tainted = value; }
            }

            public void Freeze() {
                GetInstanceData().Freeze();
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
#endif
#region Generated by Subclasses.Generator.rb
    public partial class MatchData {
        public sealed partial class Subclass : MatchData, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public bool IsFrozen {
                get { return _instanceData != null && _instanceData.Frozen; }
            }

            public bool IsTainted {
                get { return _instanceData != null && _instanceData.Tainted; }
                set { GetInstanceData().Tainted = value; }
            }

            public void Freeze() {
                GetInstanceData().Freeze();
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
    public partial class Proc {
        public sealed partial class Subclass : Proc, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public bool IsFrozen {
                get { return _instanceData != null && _instanceData.Frozen; }
            }

            public bool IsTainted {
                get { return _instanceData != null && _instanceData.Tainted; }
                set { GetInstanceData().Tainted = value; }
            }

            public void Freeze() {
                GetInstanceData().Freeze();
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
    public partial class Range {
        public sealed partial class Subclass : Range, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public bool IsFrozen {
                get { return _instanceData != null && _instanceData.Frozen; }
            }

            public bool IsTainted {
                get { return _instanceData != null && _instanceData.Tainted; }
                set { GetInstanceData().Tainted = value; }
            }

            public void Freeze() {
                GetInstanceData().Freeze();
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
    public partial class RubyRegex {
        public sealed partial class Subclass : RubyRegex, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public bool IsFrozen {
                get { return _instanceData != null && _instanceData.Frozen; }
            }

            public bool IsTainted {
                get { return _instanceData != null && _instanceData.Tainted; }
                set { GetInstanceData().Tainted = value; }
            }

            public void Freeze() {
                GetInstanceData().Freeze();
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
#endregion

#if GENERATOR
    Stateful = ['RubyArray']

    def generate
      Stateful.each do |cls| 
        @class = cls
        super 
      end
    end

    def Class *a
      @class
    end
#else
    public partial class /*$Class{*/MutableString/*}*/ {
        public sealed partial class Subclass : /*$Class{*/MutableString/*}*/, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
#endif
#region Generated by Subclasses.Generator.rb
    public partial class RubyArray {
        public sealed partial class Subclass : RubyArray, IRubyObject {
            private RubyInstanceData _instanceData;
            private RubyClass/*!*/ _immediateClass;
            
            [Emitted]
            public RubyClass/*!*/ ImmediateClass {
                get {
                    return _immediateClass;
                }
                set {
                    // once a singleton immediate class is set it can't be changed:
                    Debug.Assert((_immediateClass == null || !_immediateClass.IsSingletonClass) && value != null);
                    _immediateClass = value;
                }
            }

            public RubyInstanceData/*!*/ GetInstanceData() {
                return RubyOps.GetInstanceData(ref _instanceData);
            }

            public RubyInstanceData TryGetInstanceData() {
                return _instanceData;
            }

            public int BaseGetHashCode() {
                return base.GetHashCode();
            }

            public bool BaseEquals(object other) {
                return base.Equals(other);
            }
        }
    }
#endregion
}
