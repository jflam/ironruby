require 'rubygems/command'
require 'rubygems/version_option'
require 'rubygems/uninstaller'

module Gem
  module Commands
    class UninstallCommand < Command

      include VersionOption

      def initialize
        super 'uninstall', 'Uninstall gems from the local repository',
              :version => Gem::Requirement.default

        add_option('-a', '--[no-]all',
          'Uninstall all matching versions'
          ) do |value, options|
          options[:all] = value
        end

        add_option('-I', '--[no-]ignore-dependencies',
                   'Ignore dependency requirements while',
                   'uninstalling') do |value, options|
          options[:ignore] = value
        end

        add_option('-x', '--[no-]executables',
                     'Uninstall applicable executables without',
                     'confirmation') do |value, options|
          options[:executables] = value
        end

        add_option('-i', '--install-dir DIR',
                   'Directory to uninstall gem from') do |value, options|
          options[:install_dir] = File.expand_path(value)
        end

        add_option('-n', '--bindir DIR',
                   'Directory to remove binaries from') do |value, options|
          options[:bin_dir] = File.expand_path(value)
        end

        add_version_option
        add_platform_option
      end

      def arguments # :nodoc:
        "GEMNAME       name of gem to uninstall"
      end

      def defaults_str # :nodoc:
        "--version '#{Gem::Requirement.default}' --no-force " \
        "--install-dir #{Gem.dir}"
      end

      def usage # :nodoc:
        "#{program_name} GEMNAME [GEMNAME ...]"
      end

      def execute
        get_all_gem_names.each do |gem_name|
          begin
            Gem::Uninstaller.new(gem_name, options).uninstall
          rescue Gem::GemNotInHomeException => e
            spec = e.spec
            alert("In order to remove #{spec.name}, please execute:\n" \
                  "\tgem uninstall #{spec.name} --install-dir=#{spec.installation_path}")
          end
        end
      end
    end
  end
end
