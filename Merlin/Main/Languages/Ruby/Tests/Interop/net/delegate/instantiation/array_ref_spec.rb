require File.dirname(__FILE__) + '/../../spec_helper'
require File.dirname(__FILE__) + '/../shared/instantiation'

describe "Reference array Void delegate instantiation" do
  csc <<-EOL
    public partial class DelegateHolder {
      public delegate string[] ARefVoidDelegate();
      public delegate string[] ARefRefDelegate(string foo);
      public delegate string[] ARefValDelegate(int foo);
      public delegate string[] ARefARefDelegate(string[] foo);
      public delegate string[] ARefAValDelegate(int[] foo);
      public delegate string[] ARefGenericDelegate<T>(T foo);
    }
  EOL
  
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefVoidDelegate  
end

describe "Reference array Reference delegate" do
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefRefDelegate
end

describe "Reference array Value delegate" do
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefValDelegate
end

describe "Reference array Reference array delegate" do
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefARefDelegate
end

describe "Reference array Value array delegate" do
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefAValDelegate
end

describe "Reference array Generic delegate" do
  it_behaves_like :delegate_instantiation, DelegateHolder::ARefGenericDelegate.of(Object)
end

