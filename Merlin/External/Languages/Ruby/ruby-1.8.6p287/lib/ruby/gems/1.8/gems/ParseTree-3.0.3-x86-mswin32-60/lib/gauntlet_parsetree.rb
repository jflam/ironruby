#!/usr/bin/ruby -ws

$f ||= false

$:.unshift "../../ruby_parser/dev/lib"
$:.unshift "../../ParseTree/dev/lib"

require 'rubygems'
require 'parse_tree'
require 'ruby_parser'

require 'gauntlet'

class ParseTreeGauntlet < Gauntlet
  def initialize
    super

    self.data = Hash.new { |h,k| h[k] = {} }
    old_data = load_yaml data_file
    self.data.merge! old_data
  end

  def should_skip? name
    if $f then
      if Hash === data[name] then
        ! data[name].empty?
      else
        data[name]
      end
    else
      data[name] == true # yes, == true on purpose
    end
  end

  def diff_pp o1, o2
    require 'pp'

    File.open("/tmp/a.#{$$}", "w") do |f|
      PP.pp o1, f
    end

    File.open("/tmp/b.#{$$}", "w") do |f|
      PP.pp o2, f
    end

    `diff -u /tmp/a.#{$$} /tmp/b.#{$$}`
  ensure
    File.unlink "/tmp/a.#{$$}" rescue nil
    File.unlink "/tmp/b.#{$$}" rescue nil
  end

  def broke name, file, msg
    warn "bad"
    self.data[name][file] = msg
    self.dirty = true
  end

  def process path, name
    begin
      $stderr.print "  #{path}: "
      rp = RubyParser.new
      pt = ParseTree.new

      old_ruby = File.read(path)

      begin
        pt_sexp = pt.process old_ruby
      rescue SyntaxError => e
        warn "unparsable pt"
        self.data[name][path] = :unparsable_pt
        self.dirty = true
        return
      end

      begin
        rp_sexp = rp.process old_ruby
      rescue Racc::ParseError => e
        broke name, path, e.message
        return
      end

      if rp_sexp != pt_sexp then
        broke name, path, diff_pp(rp_sexp, pt_sexp)
        return
      end

      self.data[name][path] = true
      self.dirty = true

      warn "good"
    rescue Interrupt
      puts "User cancelled"
      exit 1
    rescue Exception => e
      broke name, path, "    UNKNOWN ERROR: #{e}: #{e.message.strip}"
    end
  end

  def run name
    warn name
    Dir["**/*.rb"].sort.each do |path|
      next if path =~ /gemspec.rb/ # HACK
      result = data[name][path]
      next if result == true || Symbol === result
      process path, name
    end

    if (self.data[name].empty? or
        self.data[name].values.all? { |v| v == true }) then
      warn "  ALL GOOD!"
      self.data[name] = true
      self.dirty = true
    end
  end
end

filter = ARGV.shift
filter = Regexp.new filter if filter

gauntlet = ParseTreeGauntlet.new
gauntlet.run_the_gauntlet filter
