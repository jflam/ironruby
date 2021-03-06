require File.dirname(__FILE__) + '/../../spec_helper'

ruby_version_is "1.8.7" do
  require 'securerandom'

  describe "SecureRandom.base64" do
    it "generates a random base64 string out of specified number of random bytes" do
      (16..128).each do |idx|
        base64 = SecureRandom.base64(idx)
        base64.class.should == String
        base64.length.should < 2 * idx
      end
      
      base64 = SecureRandom.base64(16.5)
      base64.class.should == String
      base64.length.should < 2 * 16
    end

    it "returns an empty string when argument is 0" do
      SecureRandom.base64(0).should == ""
    end

    it "generates different base64 strings with subsequent invocations" do
      # quick and dirty check, but good enough
      values = []
      256.times do
        base64 = SecureRandom.base64
        # make sure the random values are not repeating
        values.include?(base64).should == false
        values << base64
      end
    end

    it "generates a random base64 string out of 32 random bytes" do
      SecureRandom.base64.class.should == String
      SecureRandom.base64.length.should < 32 * 2
    end

    it "treats nil agrument as default one and generates a random base64 string" do
      SecureRandom.base64(nil).class.should == String
      SecureRandom.base64(nil).length.should < 32 * 2
    end

    it "raises ArgumentError on negative arguments" do
      lambda {
        SecureRandom.base64(-1)
      }.should raise_error(ArgumentError)
    end

    it "tries to convert the passed argument to an Integer using #to_int" do
      obj = mock("to_int")
      obj.should_receive(:to_int).and_return(5)
      SecureRandom.base64(obj).size.should < 10
    end
  end
end
