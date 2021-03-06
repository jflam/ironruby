require File.dirname(__FILE__) + '/../../spec_helper'
require File.dirname(__FILE__) + "/../../shared/modification"

describe "Adding methods on a Class" do
  csc <<-EOL
    public partial class Klass {
      public int BarI() {
        return 1;
      }
      
      public static int BarC() {
        return 2;
      }
    }
  EOL
  before(:each) do
    @klass = Klass
    @obj = Klass.new
    class << @obj
      def bar_s
        3
      end
    end
  end
  it_behaves_like :adding_a_method, Klass
  it_behaves_like :adding_class_methods, Klass
  it_behaves_like :adding_metaclass_methods, Klass
end
