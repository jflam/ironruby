/* ****************************************************************************
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

using EachSite = System.Func<System.Runtime.CompilerServices.CallSite, object, IronRuby.Builtins.Proc, object>;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Scripting;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Actions;
using IronRuby.Runtime;
using IronRuby.Runtime.Calls;
using System.Runtime.InteropServices;
using Ast = System.Linq.Expressions.Expression;

namespace IronRuby.Builtins {

    [RubyClass("Struct", Extends = typeof(RubyStruct), Inherits = typeof(object)), Includes(typeof(Enumerable))]
    public static partial class RubyStructOps {
        [RubyConstructor]
        public static void AllocatorUndefined(RubyClass/*!*/ self, params object[] args) {
            throw RubyExceptions.CreateAllocatorUndefinedError(self);
        }

        [RubyMethod("new", RubyMethodAttributes.PublicSingleton)]
        public static object NewAnonymousStruct(BlockParam block, RubyClass/*!*/ self, int className,
            [DefaultProtocol, NotNull, NotNullItems]params string/*!*/[]/*!*/ attributeNames) {

            return CreateAnonymousWithFirstAttribute(block, self, RubyOps.ConvertFixnumToSymbol(self.Context, className), attributeNames);
        }

        [RubyMethod("new", RubyMethodAttributes.PublicSingleton)]
        public static object NewAnonymousStruct(BlockParam block, RubyClass/*!*/ self, SymbolId className,
            [DefaultProtocol, NotNull, NotNullItems]params string/*!*/[]/*!*/ attributeNames) {

            return CreateAnonymousWithFirstAttribute(block, self, RubyOps.ConvertSymbolIdToSymbol(className), attributeNames);
        }

        [RubyMethod("new", RubyMethodAttributes.PublicSingleton)]
        public static object NewAnonymousStruct(BlockParam block, RubyClass/*!*/ self, [NotNull]string/*!*/ className,
            [DefaultProtocol, NotNull, NotNullItems]params string/*!*/[]/*!*/ attributeNames) {

            return CreateAnonymousWithFirstAttribute(block, self, className, attributeNames);
        }

        [RubyMethod("new", RubyMethodAttributes.PublicSingleton)]
        public static object NewStruct(BlockParam block, RubyClass/*!*/ self, [DefaultProtocol, Optional]MutableString className,
            [DefaultProtocol, NotNull, NotNullItems]params string/*!*/[]/*!*/ attributeNames) {

            if (className == null) {
                return Create(block, self, null, attributeNames);
            }

            string strName = className.ConvertToString();
            self.Context.CheckConstantName(strName);
            return Create(block, self, strName, attributeNames);
        }

        public static object CreateAnonymousWithFirstAttribute(BlockParam block, RubyClass/*!*/ self,
            string/*!*/ firstAttribute, string/*!*/[]/*!*/ attributeNames) {

            return Create(block, self, null, ArrayUtils.Insert(firstAttribute, attributeNames));
        }

        /// <summary>
        /// Struct#new
        /// Creates Struct classes with the specified name and members
        /// </summary>
        private static object Create(BlockParam block, RubyClass/*!*/ self, string className, string/*!*/[]/*!*/ attributeNames) {
            var result = RubyStruct.DefineStruct(self, className, attributeNames);

            if (block != null) {
                return RubyUtils.EvaluateInModule(result, block, result);
            }

            return result;
        }

        // Reinitialization. Called only from derived struct's initializer.
        [RubyMethod("initialize", RubyMethodAttributes.PrivateInstance)]
        public static void Reinitialize(RubyStruct/*!*/ self, [NotNull]params object[]/*!*/ items) {
            self.SetValues(items);
        }

        // Copies data from one Struct instance into another:
        [RubyMethod("initialize_copy", RubyMethodAttributes.PrivateInstance)]
        public static RubyStruct/*!*/ InitializeCopy(RubyStruct/*!*/ self, [NotNull]RubyStruct/*!*/ source) {
            // TODO: compare non-singleton classes?
            if (self.ImmediateClass.GetNonSingletonClass() != source.ImmediateClass.GetNonSingletonClass()) {
                throw RubyExceptions.CreateTypeError("wrong argument class");
            }

            self.SetValues(source.Values);
            return self;
        }

        [RubyMethod("members")]
        public static RubyArray/*!*/ GetMembers(RubyStruct/*!*/ self) {
            return RubyStruct.GetMembers(self);
        }

        [RubyMethod("length")]
        [RubyMethod("size")]
        public static int GetSize(RubyStruct/*!*/ self) {
            return self.ItemCount;
        }

        [RubyMethod("[]")]
        public static object GetValue(RubyStruct/*!*/ self, int index) {
            return self[NormalizeIndex(self.ItemCount, index)];
        }

        [RubyMethod("[]")]
        public static object GetValue(RubyStruct/*!*/ self, SymbolId name) {
            return self[SymbolTable.IdToString(name)];
        }

        [RubyMethod("[]")]
        public static object GetValue(RubyStruct/*!*/ self, MutableString/*!*/ name) {
            return self[name.ConvertToString()];
        }

        [RubyMethod("[]")]
        public static object GetValue(ConversionStorage<int>/*!*/ conversionStorage, RubyStruct/*!*/ self, object index) {
            return self[NormalizeIndex(self.ItemCount, Protocols.CastToFixnum(conversionStorage, index))];
        }

        [RubyMethod("[]=")]
        public static object SetValue(RubyStruct/*!*/ self, int index, object value) {
            return self[NormalizeIndex(self.ItemCount, index)] = value;
        }

        [RubyMethod("[]=")]
        public static object SetValue(RubyStruct/*!*/ self, SymbolId name, object value) {
            return self[SymbolTable.IdToString(name)] = value;
        }

        [RubyMethod("[]=")]
        public static object SetValue(RubyStruct/*!*/ self, MutableString/*!*/ name, object value) {
            return self[name.ConvertToString()] = value;
        }

        [RubyMethod("[]=")]
        public static object SetValue(ConversionStorage<int>/*!*/ conversionStorage, RubyStruct/*!*/ self, object index, object value) {
            return self[NormalizeIndex(self.ItemCount, Protocols.CastToFixnum(conversionStorage, index))] = value;
        }

        [RubyMethod("each")]
        public static object Each(BlockParam block, RubyStruct/*!*/ self) {
            if (block == null && self.ItemCount > 0) {
                throw RubyExceptions.NoBlockGiven();
            }

            foreach (var value in self.Values) {
                object result;
                if (block.Yield(value, out result)) {
                    return result;
                }
            }

            return self;
        }

        [RubyMethod("each_pair")]
        public static object EachPair(BlockParam block, RubyStruct/*!*/ self) {
            if (block == null && self.ItemCount > 0) {
                throw RubyExceptions.NoBlockGiven();
            }

            foreach (KeyValuePair<string, object> entry in self.GetItems()) {
                object result;
                if (block.Yield(SymbolTable.StringToId(entry.Key), entry.Value, out result)) {
                    return result;
                }
            }

            return self;
        }

        [RubyMethod("to_a")]
        [RubyMethod("values")]
        public static RubyArray/*!*/ Values(RubyStruct/*!*/ self) {
            return new RubyArray(self.Values);
        }

        [RubyMethod("hash")]
        public static int Hash(UnaryOpStorage/*!*/ hashStorage, ConversionStorage<int>/*!*/ fixnumCast, RubyStruct/*!*/ self) {
            return self.GetHashCode(hashStorage, fixnumCast);
        }

        [RubyMethod("eql?")]
        public static bool Equal(BinaryOpStorage/*!*/ eqlStorage, RubyStruct/*!*/ self, object other) {
            return self.Equals(eqlStorage, other);
        }

        // same pattern as RubyStruct.Equals, but we need to call == instead of eql?
        [RubyMethod("==")]
        public static bool Equals(BinaryOpStorage/*!*/ equals, RubyStruct/*!*/ self, object obj) {
            var other = obj as RubyStruct;
            if (!self.StructReferenceEquals(other)) {
                return false;
            }
            Debug.Assert(self.ItemCount == other.ItemCount);

            if (self.Values.Length > 0) {
                var site = equals.GetCallSite("==");
                for (int i = 0; i < self.Values.Length; i++) {
                    if (RubyOps.IsFalse(site.Target(site, self.Values[i], other.Values[i]))) {
                        return false;
                    }
                }
            }

            return true;
        }

        [RubyMethod("to_s")]
        [RubyMethod("inspect")]
        public static MutableString/*!*/ Inspect(RubyStruct/*!*/ self) {
            RubyContext context = self.ImmediateClass.Context;

            using (IDisposable handle = RubyUtils.InfiniteInspectTracker.TrackObject(self)) {
                // #<struct Struct::Foo name=nil, val=nil>
                var result = MutableString.CreateMutable();
                result.Append("#<struct ");
                result.Append(context.Inspect(context.GetClassOf(self)));

                if (handle == null) {
                    return result.Append(":...>");
                }
                result.Append(' ');

                object[] data = self.Values;
                var members = self.GetNames();
                for (int i = 0; i < data.Length; i++) {
                    if (i != 0) {
                        result.Append(", ");
                    }
                    result.Append(members[i]);
                    result.Append("=");
                    result.Append(context.Inspect(data[i]));
                }
                result.Append('>');
                return result;
            }
        }

        // For some unknown reason Struct defines the method even though it is mixed in from Enumerable
        // Until we discover the difference, delegate to Enumerable#select
        [RubyMethod("select")]
        public static RubyArray/*!*/ Select(CallSiteStorage<EachSite>/*!*/ each, BlockParam predicate, RubyStruct/*!*/ self) {
            return Enumerable.Select(each, predicate, self);
        }

        // equivalent to Array#values_at over the data array
        [RubyMethod("values_at")]
        public static RubyArray/*!*/ ValuesAt(ConversionStorage<int>/*!*/ fixnumCast, RubyStruct/*!*/ self, [NotNull]params object[] values) {
            RubyArray result = new RubyArray();
            object[] data = self.Values;

            for (int i = 0; i < values.Length; ++i) {
                Range range = values[i] as Range;
                if (range != null) {
                    int begin = Protocols.CastToFixnum(fixnumCast, range.Begin);
                    int end = Protocols.CastToFixnum(fixnumCast, range.End);

                    if (range.ExcludeEnd) {
                        end -= 1;
                    }

                    begin = NormalizeIndex(data.Length, begin);
                    end = NormalizeIndex(data.Length, end);
                    Debug.Assert(end - begin <= data.Length); // because we normalized the indicies

                    if (end - begin > 0) {
                        result.Capacity += (end - begin);
                        for (int j = begin; j <= end; j++) {
                            result.Add(data[j]);
                        }
                    }
                } else {
                    int index = NormalizeIndex(data.Length, Protocols.CastToFixnum(fixnumCast, values[i]));
                    result.Add(data[index]);
                }
            }

            return result;
        }

        private static int NormalizeIndex(int itemCount, int index) {
            int normalized = index;
            if (normalized < 0) {
                normalized += itemCount;
            }
            if (normalized >= 0 && normalized < itemCount) {
                return normalized;
            }
            // MRI reports the normalized index, but we'll report the original one
            throw RubyExceptions.CreateIndexError(String.Format("offset {0} too small for struct (size:{1})", index, itemCount));
        }
    }
}
